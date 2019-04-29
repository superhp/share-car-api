using AutoMapper.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using ShareCar.Db.Entities;
using ShareCar.Db.Repositories.User_Repository;
using ShareCar.Dto;
using ShareCar.Dto.Identity;
using ShareCar.Dto.Identity.Cognizant;
using ShareCar.Logic.User_Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ShareCar.Logic.Identity_Logic
{
    public class CognizantIdentity : ICognizantIdentity
    {
        private readonly IUserLogic _userlogic;
        private readonly SendGridClient _client;
        private readonly SendGridSettings _sgSettings;
        private readonly IUserRepository _userRepository;
        private readonly IJwtFactory _jwtFactory;

        public CognizantIdentity(IUserLogic userlogic, IOptions<SendGridSettings> sgSettings, IUserRepository userRepository, IJwtFactory jwtFactory)
        {
            _sgSettings = sgSettings.Value;
            _client = new SendGridClient(_sgSettings.APIKey);
            _userlogic = userlogic;
            _userRepository = userRepository;
            _jwtFactory = jwtFactory;
        }

        public async Task<string> SubmitVerificationCodeAsync(VerificationCodeSubmitData data)
        {
            string loginEmail = data.FacebookEmail != null ? data.FacebookEmail : data.GoogleEmail;
            UnauthorizedUserDto user = _userlogic.GetUnauthorizedUser(loginEmail);

            if (user.VerificationCode != data.VerificationCode)
                return null;

            string originalLoginEmail = GetOriginalLoginEmail(loginEmail);

            _userlogic.VerifyUser(data.FacebookEmail != null, originalLoginEmail);

            var localUser = _userRepository.GetUserByEmail(EmailType.LOGIN, originalLoginEmail);
            var jwtIdentity = _jwtFactory.GenerateClaimsIdentity(localUser.UserName, localUser.Id);
            var jwt = await _jwtFactory.GenerateEncodedToken(localUser.UserName, jwtIdentity);

            return jwt;
        }

        // When user logs in with second email for the first time, he temporarely has two accounts, although only one should be used.
        // The one which will be left has all types of emails set.
        private string GetOriginalLoginEmail(string loginEmail)
        {
            var facebookAcc = _userlogic.GetUserByEmail(EmailType.FACEBOOK, loginEmail);
            if (facebookAcc != null && facebookAcc.CognizantEmail != null)
            {
                return facebookAcc.Email;
            }

            var googleAcc = _userlogic.GetUserByEmail(EmailType.GOOGLE, loginEmail);

            if (googleAcc != null && googleAcc.CognizantEmail != null)
            {
                return googleAcc.Email;
            }

            throw new ArgumentException("User not found.");

        }

        public async Task SendVerificationCode(string cognizantEmail, string loginEmail)
        {
            int code = _userlogic.GetUnauthorizedUser(loginEmail).VerificationCode;
            var msg = new SendGridMessage();

            msg.SetFrom(new EmailAddress("cts.share.car@gmail.com", "Share car"));

            var recipients = new List<EmailAddress>
            {
                new EmailAddress(cognizantEmail)
            };
            msg.AddTos(recipients);

            msg.SetSubject("Verification code");

            msg.AddContent(MimeType.Text, "Your verification code is " + code.ToString());

            var response = await _client.SendEmailAsync(msg);
            var tt=0;
        }
    }
}
