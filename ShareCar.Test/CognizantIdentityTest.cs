using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SendGrid;
using ShareCar.Db.Entities;
using ShareCar.Db.Repositories.User_Repository;
using ShareCar.Dto;
using ShareCar.Dto.Identity;
using ShareCar.Dto.Identity.Cognizant;
using ShareCar.Logic.Identity_Logic;
using ShareCar.Logic.User_Logic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShareCar.Test
{
    public class CognizantIdentityTest
    {
        Mock<IUserLogic> userLogic = new Mock<IUserLogic>();
        Mock<SendGridClient> client = new Mock<SendGridClient>();
        Mock<IOptions<SendGridSettings>> sgSettings = new Mock<IOptions<SendGridSettings>>();
        Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
        Mock<IJwtFactory> jwtFactory = new Mock<IJwtFactory>();

        [Test]
        public async Task SubmitVerificationCode_FacebookEmail_ReturnsNotNull()
        {
            string token = "token";
            string facebookEmail = "facebook@facebook.com";
            int verificationCode = 1;
            User user = new User
            {
                UserName = facebookEmail,
                CognizantEmail = "cognizant@cognizant.com"
            };
            UserDto userDto = new UserDto
            {
                Email = facebookEmail,
                CognizantEmail = "cognizant@cognizant.com"
            };
            UnauthorizedUserDto unauthorizedUser = new UnauthorizedUserDto
            {
                VerificationCode = verificationCode,
                Email = facebookEmail
            };
            sgSettings.SetupGet(x => x.Value).Returns(new SendGridSettings { APIKey = "key"});
            userRepository.Setup(x => x.GetUserByEmail(EmailType.LOGIN, facebookEmail)).Returns(user);
            userLogic.Setup(x => x.GetUserByEmail(EmailType.FACEBOOK, facebookEmail)).Returns(userDto);
            userLogic.Setup(x => x.GetUnauthorizedUser(facebookEmail)).Returns(unauthorizedUser);
            jwtFactory.Setup(x => x.GenerateEncodedToken(facebookEmail, It.IsAny<System.Security.Claims.ClaimsIdentity>())).Returns(Task.FromResult(token));

            var cognizantIdentity = new CognizantIdentity(userLogic.Object, sgSettings.Object, userRepository.Object, jwtFactory.Object);

            var codeData = new VerificationCodeSubmitData { FacebookEmail = facebookEmail, VerificationCode = verificationCode };

            Assert.AreEqual(await cognizantIdentity.SubmitVerificationCodeAsync(codeData), token);

        }
    }
}
