using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using ShareCar.Db.Entities;
using ShareCar.Db.Repositories.User_Repository;
using ShareCar.Dto.Identity;
using ShareCar.Dto.Identity.Google;
using ShareCar.Logic.Identity_Logic;
using ShareCar.Logic.User_Logic;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShareCar.Test
{
    public class GoogleIdentityTest
    {
        Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
        Mock<IJwtFactory> jwtFactory = new Mock<IJwtFactory>();
        GoogleUserDataDto loginData = new GoogleUserDataDto
        {
            Id = 1,
            Email = "test@test.com",
            GivenName = "Test",
            FamilyName = "Test",
            ImageUrl = "..."
        };

        [Test]
        public async Task Login_NewUser_ReturnsNull()
        {

            User user = null;

            userRepository.Setup(x => x.GetUserByEmail(Dto.EmailType.GOOGLE, loginData.Email)).Returns(user);

           var googleIdentity = new GoogleIdentity(jwtFactory.Object, userRepository.Object);

            Assert.IsNull(await googleIdentity.Login(loginData));

        }

        [Test]
        public async Task Login_ExistingUnverifiedUser_ReturnsNull()
        {

            User user = new User {
                GoogleVerified = false
            };

            userRepository.Setup(x => x.GetUserByEmail(Dto.EmailType.GOOGLE, loginData.Email)).Returns(user);

            var googleIdentity = new GoogleIdentity(jwtFactory.Object, userRepository.Object);

            Assert.IsNull(await googleIdentity.Login(loginData));
        }

        [Test]
        public async Task Login_VerifiedUser_ReturnsNotNull()
        {
            string token = "token";
            User user = new User
            {
                Id = "1",
                UserName = loginData.Email,
                Email = loginData.Email,
                GoogleVerified = true
            };

            userRepository.Setup(x => x.GetUserByEmail(Dto.EmailType.GOOGLE, loginData.Email)).Returns(user);
            userRepository.Setup(x => x.GetUserByEmail(Dto.EmailType.LOGIN, loginData.Email)).Returns(user);
            jwtFactory.Setup(x => x.GenerateEncodedToken(loginData.Email, It.IsAny<ClaimsIdentity>())).Returns(Task.FromResult(token));

            var googleIdentity = new GoogleIdentity(jwtFactory.Object, userRepository.Object);

            Assert.AreEqual(await googleIdentity.Login(loginData), token);
        }
    }
}