global using UserCreationResult = NoteBin.Result<NoteBin.Models.User, NoteBin.Models.UserCreationError>;

using NoteBin.Models;
using NoteBin.Models.API;
using NoteBin.Services;
using System.Threading.Tasks;

namespace NoteBin.Test.Services
{
    public abstract class IUserDbServiceTests<T> where T : IUserDbService
    {
        protected T service = default!;

        private static AuthRequest UserData0 => new AuthRequest
        {
            Username = "user0",
            Password = "Password0"
        };

        [TestMethod]
        public async Task CreateUser_Success()
        {
            UserCreationResult result = await service.CreateUser(UserData0);
            Assert.IsTrue(result.IsOk);
            Assert.AreEqual(UserData0.Username, result.Value.Name);
        }

        [TestMethod]
        public async Task CreateUser_Duplicate()
        {
            await service.CreateUser(UserData0);
            UserCreationResult result = await service.CreateUser(UserData0);
            Assert.IsFalse(result.IsOk);
            Assert.AreEqual(UserCreationError.DuplicateUsername, result.Error);
        }

        [TestMethod]
        public async Task CreateUser_InvalidUsername()
        {
            UserCreationResult result = await service.CreateUser(new AuthRequest
            {
                Username = "Contains space",
                Password = UserData0.Password
            });
            Assert.IsFalse(result.IsOk);
            Assert.AreEqual(UserCreationError.InvalidUsername, result.Error);
        }

        [TestMethod]
        public async Task CreateUser_InvalidPassword()
        {
            UserCreationResult result = await service.CreateUser(new AuthRequest
            {
                Username = UserData0.Username,
                Password = "simple"
            });
            Assert.IsFalse(result.IsOk);
            Assert.AreEqual(UserCreationError.InvalidPassword, result.Error);
        }

        [TestMethod]
        public async Task GetUser_Exists()
        {
            await service.CreateUser(UserData0);
            User? user = await service.GetUser(UserData0.Username);
            Assert.IsNotNull(user);
            Assert.AreEqual(UserData0.Username, user.Name);
        }

        [TestMethod]
        public async Task GetUser_NotExists()
        {
            User? user = await service.GetUser("username");
            Assert.IsNull(user);
        }

        [TestMethod]
        public async Task VerifyUser_CorrectPassword()
        {
            await service.CreateUser(UserData0);
            User? user = await service.GetUser(UserData0.Username);
            Assert.IsNotNull(user);
            bool isValid = service.VerifyUser(user, UserData0.Password);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public async Task VerifyUser_IncorrectPassword()
        {
            await service.CreateUser(UserData0);
            User? user = await service.GetUser(UserData0.Username);
            Assert.IsNotNull(user);
            bool isValid = service.VerifyUser(user, "wrong password");
            Assert.IsFalse(isValid);
        }
    }
}
