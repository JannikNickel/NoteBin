using Microsoft.AspNetCore.Mvc;
using NoteBin.API;
using NoteBin.Models.API;
using NoteBin.Services;
using System.Threading.Tasks;

namespace NoteBin.Test.API
{
    [TestClass]
    public class AuthControllerTests
    {
        private IUserDbService userService = null!;
        private IAuthService authService = null!;
        private AuthController authController = null!;

        private static AuthRequest UserRequest0 => new AuthRequest()
        {
            Username = "TestUser",
            Password = "ValidPassword123"
        };

        [TestInitialize]
        public void Setup()
        {
            userService = TestHelper.Services.UserDb;
            authService = TestHelper.Services.Auth(userService);
            authController = new AuthController(userService, authService);
        }

        [TestMethod]
        public async Task Login_ReturnsOk_WhenCredentialsAreValid()
        {
            await userService.CreateUser(UserRequest0);
            IActionResult result = await authController.Login(UserRequest0);
            Assert.IsInstanceOfType<OkObjectResult>(result);
        }

        [TestMethod]
        public async Task Login_ReturnsUnauthorized_WhenUserMissing()
        {
            IActionResult result = await authController.Login(UserRequest0);
            Assert.IsInstanceOfType<UnauthorizedResult>(result);
        }

        [TestMethod]
        public async Task Login_ReturnsUnauthorized_WhenCredentialsAreInvalid()
        {
            await userService.CreateUser(UserRequest0);
            IActionResult result = await authController.Login(new AuthRequest
            {
                Username = UserRequest0.Username,
                Password = "wrong password"
            });
            Assert.IsInstanceOfType<UnauthorizedResult>(result);
        }

        [TestMethod]
        public async Task Validate_ReturnsOk_WhenTokenIsValid()
        {
            string? token = await CreateUserAndLogin(UserRequest0);
            authController.ControllerContext = TestHelper.HttpContextForToken(token);
            IActionResult result = await authController.Validate();
            Assert.IsInstanceOfType<OkResult>(result);
        }

        [TestMethod]
        public async Task Validate_ReturnsUnauthorized_WhenTokenIsInvalid()
        {
            authController.ControllerContext = TestHelper.HttpContextForToken("invalid_token");
            IActionResult result = await authController.Validate();
            Assert.IsInstanceOfType<UnauthorizedResult>(result);
        }

        [TestMethod]
        public async Task Logout_ReturnsOk_WhenTokenIsValid()
        {
            string? token = await CreateUserAndLogin(UserRequest0);
            authController.ControllerContext = TestHelper.HttpContextForToken(token);
            IActionResult result = await authController.Logout();
            Assert.IsInstanceOfType<OkResult>(result);
        }

        [TestMethod]
        public async Task Logout_ReturnsUnauthorized_WhenTokenIsInvalid()
        {
            authController.ControllerContext = TestHelper.HttpContextForToken("invalid_token");
            IActionResult result = await authController.Logout();
            Assert.IsInstanceOfType<UnauthorizedResult>(result);
        }

        private async Task<string?> CreateUserAndLogin(AuthRequest request)
        {
            await userService.CreateUser(request);
            OkObjectResult? loginResult = await authController.Login(request) as OkObjectResult;
            return (loginResult?.Value as AuthResponse)?.Token;
        }
    }
}
