using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ShareCar.Db.Entities;
using ShareCar.Db.Repositories.User_Repository;
using ShareCar.Dto.Identity;
using ShareCar.Dto.Identity.Facebook;
using ShareCar.Dto.Identity.Google;
using ShareCar.Logic.User_Logic;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ShareCar.Logic.Identity_Logic
{
    public class GoogleIdentity : IGoogleIdentity
    {
        private readonly IJwtFactory _jwtFactory;
        private readonly IUserRepository _userRepository;
        private static readonly HttpClient Client = new HttpClient();

       public GoogleIdentity(IJwtFactory jwtFactory, IUserRepository userRepository)
        {

            _jwtFactory = jwtFactory;
            _userRepository = userRepository;
        }
        
        public async Task<string> Login(GoogleUserDataDto userInfo)
        {

            var user = _userRepository.GetUserByEmail(Dto.EmailType.LOGIN, userInfo.Email);

            if (user != null)
            {
                if (!user.GoogleVerified)
                {
                    user.GoogleEmail = userInfo.Email;

                    if (user.FacebookVerified)
                    {
                        user.GoogleVerified = true;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {

                user = _userRepository.GetUserByEmail(Dto.EmailType.GOOGLE, userInfo.Email);
                if (user == null)
                {
                    await _userRepository.CreateUser(new User
                    {
                        FirstName = userInfo.GivenName,
                        LastName = userInfo.FamilyName,
                        Email = userInfo.Email,
                        PictureUrl = userInfo.ImageUrl,
                        FacebookVerified = false,
                        GoogleVerified = false,
                        GoogleEmail = userInfo.Email,
                        FacebookEmail = null
                    });
                    _userRepository.CreateUnauthorizedUser(new UnauthorizedUser { Email = userInfo.Email });

                    return null;
                }

                if (!user.GoogleVerified)
                {
                    return null;
                }
            }

            return await GenerateJwt(user);
        }

        private async Task<string> GenerateJwt(User localUser)
        {
            var jwtIdentity = _jwtFactory.GenerateClaimsIdentity(localUser.UserName, localUser.Id);
            var jwt = await _jwtFactory.GenerateEncodedToken(localUser.UserName, jwtIdentity);

            return jwt;
        }
    }
}
