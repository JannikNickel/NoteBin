using Microsoft.AspNetCore.Mvc;
using NoteBin.API;
using NoteBin.Models.API;
using NoteBin.Services;
using System.Threading.Tasks;

namespace NoteBin.Test.API
{
    [TestClass]
    public class UserControllerTests
    {
        private IUserDbService userService = null!;
        private UserController userController = null!;

        private static AuthRequest UserRequest0 => new AuthRequest()
        {
            Username = "TestUser",
            Password = "ValidPassword123"
        };

        [TestInitialize]
        public void Setup()
        {
            userService = TestHelper.Services.UserDb;
            userController = new UserController(userService);
        }

        [TestMethod]
        public async Task CreateUser_ReturnsOk_WhenUserIsCreatedSuccessfully()
        {
            IActionResult result = await userController.CreateUser(UserRequest0);
            Assert.IsInstanceOfType<OkResult>(result);
        }

        [TestMethod]
        public async Task CreateUser_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            userController.ModelState.AddModelError("Username", "Required");
            IActionResult result = await userController.CreateUser(null);
            Assert.IsInstanceOfType<BadRequestObjectResult>(result);
        }

        [TestMethod]
        public async Task GetUserByName_ReturnsOk_WhenUserExists()
        {
            await userService.CreateUser(UserRequest0);
            IActionResult result = await userController.GetUserByName(UserRequest0.Username);
            Assert.IsInstanceOfType<OkObjectResult>(result);
        }

        [TestMethod]
        public async Task GetUserByName_ReturnsNotFound_WhenUserDoesNotExist()
        {
            IActionResult result = await userController.GetUserByName("missing");
            Assert.IsInstanceOfType<NotFoundResult>(result);
        }
    }
}
