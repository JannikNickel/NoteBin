using NoteBin.Models;
using NoteBin.Models.API;
using NoteBin.Services;
using System.Threading.Tasks;

namespace NoteBin.Test.Services
{
    public abstract class IAuthServiceTests<T> where T : IAuthService
    {
        protected IUserDbService userService = null!;
        protected T service = default!;

        private static User TestUser => TestHelper.FakeUser("username");
        private static AuthRequest TestUserAuthReq => new AuthRequest()
        {
            Username = TestUser.Name,
            Password = TestUser.Password
        };

        [TestMethod]
        public async Task GenerateToken_Success()
        {
            string token = await service.GenerateToken(TestUser);
            Assert.IsFalse(string.IsNullOrEmpty(token));
        }

        [TestMethod]
        public async Task ValidateToken_ValidToken()
        {
            await userService.CreateUser(TestUserAuthReq);
            string token = await service.GenerateToken(TestUser);
            User? user = await service.ValidateToken(token);
            Assert.IsNotNull(user);
            Assert.AreEqual(TestUser.Name, user.Name);
        }

        [TestMethod]
        public async Task ValidateToken_WrongUser()
        {
            await userService.CreateUser(TestUserAuthReq);
            string token = await service.GenerateToken(TestHelper.FakeUser("newUser"));
            User? user = await service.ValidateToken(token);
            Assert.IsNull(user);
        }

        [TestMethod]
        public async Task ValidateToken_InvalidToken()
        {
            User? user = await service.ValidateToken("invalidToken");
            Assert.IsNull(user);
        }

        [TestMethod]
        public async Task Logout_Success()
        {
            await userService.CreateUser(TestUserAuthReq);
            string token = await service.GenerateToken(TestUser);
            bool result = await service.Logout(token);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task Logout_MissingUser()
        {
            string token = await service.GenerateToken(TestHelper.FakeUser("newUser"));
            bool result = await service.Logout(token);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task Logout_InvalidToken()
        {
            bool result = await service.Logout("invalidToken");
            Assert.IsFalse(result);
        }
    }
}
