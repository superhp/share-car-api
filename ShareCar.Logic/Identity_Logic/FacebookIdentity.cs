using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ShareCar.Db.Entities;
using ShareCar.Db.Repositories;
using ShareCar.Db.Repositories.User_Repository;
using ShareCar.Dto;
using ShareCar.Dto.Identity;
using ShareCar.Dto.Identity.Facebook;
using ShareCar.Logic.User_Logic;

namespace ShareCar.Logic.Identity_Logic
{
    public class FacebookIdentity : IFacebookIdentity
    {
        private readonly FacebookAuthSettings _fbAuthSettings;
        private readonly IJwtFactory _jwtFactory;
        private readonly IUserRepository _userRepository;
        private static readonly HttpClient Client = new HttpClient();

        public FacebookIdentity(IOptions<FacebookAuthSettings> fbAuthSettings, IJwtFactory jwtFactory, IUserRepository userRepository)
        {
            _fbAuthSettings = fbAuthSettings.Value;
            _jwtFactory = jwtFactory;
            _userRepository = userRepository;
        }

        private async Task<FacebookUserDataDto> GetUserFromFacebook(AccessTokenDto facebookAccessToken)
        {
            // generate an app access token
            var appAccessTokenUrl = _fbAuthSettings.AppAccessTokenUrl
                .Replace("{AppId}", _fbAuthSettings.AppId)
                .Replace("{AppSecret}", _fbAuthSettings.AppSecret);
            var appAccessTokenResponse = await Client.GetStringAsync(appAccessTokenUrl);
            var appAccessToken = JsonConvert.DeserializeObject<FacebookAppAccessTokenDto>(appAccessTokenResponse);

            // validate the user access token
            await CheckIfAccessTokenIsValid(facebookAccessToken, appAccessToken);

            // we've got a valid token so we can request user data from facebook
            var userInfoUrl = _fbAuthSettings.UserInfoUrl.Replace("{FacebookAccessToken}", facebookAccessToken.AccessToken);
            var userInfoResponse = await Client.GetStringAsync(userInfoUrl);
            var userInfo = JsonConvert.DeserializeObject<FacebookUserDataDto>(userInfoResponse);

            return userInfo;
        }

        private async Task CheckIfAccessTokenIsValid(AccessTokenDto facebookAccessToken, FacebookAppAccessTokenDto appAccessToken)
        {
            var debugTokenUrl = _fbAuthSettings.DebugTokenUrl
                .Replace("{FacebookAccessToken}", facebookAccessToken.AccessToken)
                .Replace("{AccessToken}", appAccessToken.AccessToken);
            var userAccessTokenValidationResponse = await Client.GetStringAsync(debugTokenUrl);
            var userAccessTokenValidation =
                JsonConvert.DeserializeObject<FacebookUserAccessTokenValidation>(userAccessTokenValidationResponse);

            if (!userAccessTokenValidation.Data.IsValid)
            {
                throw new ArgumentException("Invalid facebook token.");
            }
        }

        private async Task<string> GenerateJwt(User localUser)
        {
            var jwtIdentity = _jwtFactory.GenerateClaimsIdentity(localUser.UserName, localUser.Id);
            var jwt = await _jwtFactory.GenerateEncodedToken(localUser.UserName, jwtIdentity);

            return jwt;
        }

        public async Task<string> Login(AccessTokenDto accessToken)
        {
            var userInfo = await GetUserFromFacebook(accessToken);
            // ready to create the local user account (if necessary) and jwt

            var user = _userRepository.GetUserByEmail(EmailType.LOGIN, userInfo.Email);

            if (user != null)
            {
                if (!user.FacebookVerified)
                {
                    user.FacebookEmail = userInfo.Email;

                    if (user.GoogleVerified)
                    {
                        user.FacebookVerified = true;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {

                user = _userRepository.GetUserByEmail(EmailType.FACEBOOK, userInfo.Email);
                if (user == null)
                {
                    await _userRepository.CreateUser(new User
                    {
                        FirstName = userInfo.FirstName,
                        LastName = userInfo.LastName,
                        Email = userInfo.Email,
                        PictureUrl = userInfo.Picture.Data.Url,
                        FacebookVerified = false,
                        GoogleVerified = false,
                        FacebookEmail = userInfo.Email,
                        GoogleEmail = null
                    });
                    _userRepository.CreateUnauthorizedUser(new UnauthorizedUser { Email = userInfo.Email });

                    return null;
                }
                if (!user.FacebookVerified)
                {
                    return null;
                }
            }


            return await GenerateJwt(user);
        }
    }
}