using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using ShareCar.Db.Entities;
using ShareCar.Db.Repositories.User_Repository;
using ShareCar.Dto.Identity;
using ShareCar.Dto.Identity.Facebook;
using ShareCar.Logic.Identity_Logic;
using ShareCar.Logic.User_Logic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShareCar.Test
{
    public class FacebookIdentityTest
    {
        Mock<IUserRepository> userRepository = new Mock<IUserRepository>();
        Mock<IJwtFactory> jwtFactory = new Mock<IJwtFactory>();
        Mock<IOptions<FacebookAuthSettings>> fbAuthSettings = new Mock<IOptions<FacebookAuthSettings>>();
        [Test]
        public async Task Login_ExistingUser_ReturnsNotNull()
        {

            User user = new User 
            {
                Email = "test@test.com"
            };

            userRepository.Setup(x => x.GetUserByEmail(Dto.EmailType.FACEBOOK, user.Email)).Returns(user);

            var facebookIdentity = new FacebookIdentity(fbAuthSettings.Object, jwtFactory.Object, userRepository.Object);

            Assert.IsNull(await facebookIdentity.Login(new AccessTokenDto { AccessToken = "token"}));

        }
    }
}
