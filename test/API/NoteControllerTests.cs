using Microsoft.AspNetCore.Mvc;
using NoteBin.API;
using NoteBin.Models;
using NoteBin.Models.API;
using NoteBin.Services;
using System.Threading.Tasks;

namespace NoteBin.Test.API
{
    [TestClass]
    public class NoteControllerTests
    {
        private IUserDbService userService = null!;
        private IAuthService authService = null!;
        private INoteDbService noteService = null!;
        private NoteController noteController = null!;

        [TestInitialize]
        public void Setup()
        {
            userService = TestHelper.Services.UserDb;
            authService = TestHelper.Services.Auth(userService);
            noteService = TestHelper.Services.NoteDb;
            noteController = new NoteController(noteService, authService);
            noteController.ControllerContext = TestHelper.EmptyHttpContext();
        }

        private static NoteCreateRequest NoteRequest0 => new NoteCreateRequest()
        {
            Name = "TestNote",
            Syntax = "plaintext",
            Content = "Test note content"
        };

        [TestMethod]
        public async Task CreateUnownedNote_ReturnsOk_WhenNoteIsCreatedSuccessfully()
        {
            IActionResult result = await noteController.CreateNote(NoteRequest0);
            Assert.IsInstanceOfType<OkObjectResult>(result);
        }

        [TestMethod]
        public async Task CreateOwnedNote_ReturnsNoteIdWithOwner()
        {
            string username = "testuser";
            UserCreationResult userRes = await userService.CreateUser(new AuthRequest
            {
                Username = username,
                Password = "ValidPassword123"
            });
            string token = await authService.GenerateToken(userRes.Value);
            noteController.ControllerContext = TestHelper.HttpContextForToken(token);

            OkObjectResult? result = await noteController.CreateNote(NoteRequest0) as OkObjectResult;
            NoteCreateResponse? response = result?.Value as NoteCreateResponse;
            Assert.IsNotNull(response);

            Note? note = await noteService.GetNote(response.Id);
            Assert.AreEqual(username, note?.Owner);
        }

        [TestMethod]
        public async Task CreateNote_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            noteController.ModelState.AddModelError("Content", "Required");
            IActionResult result = await noteController.CreateNote(null);
            Assert.IsInstanceOfType<BadRequestObjectResult>(result);
        }

        [TestMethod]
        public async Task GetNoteById_ReturnsOk_WhenNoteExists()
        {
            Note? saved = await noteService.SaveNote(NoteRequest0, null);
            IActionResult result = await noteController.GetNoteById(saved!.Id);
            Assert.IsInstanceOfType<OkObjectResult>(result);
        }

        [TestMethod]
        public async Task GetNoteById_ReturnsNotFound_WhenNoteDoesNotExist()
        {
            IActionResult result = await noteController.GetNoteById("missing");
            Assert.IsInstanceOfType<NotFoundResult>(result);
        }

        [TestMethod]
        public async Task ListNotes_ReturnsOk_WithValidRequest()
        {
            NoteListRequest request = new NoteListRequest { Offset = 0, Amount = 10 };
            IActionResult result = await noteController.ListNotes(request);
            Assert.IsInstanceOfType<OkObjectResult>(result);
        }

        [TestMethod]
        public async Task ListNotes_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            noteController.ModelState.AddModelError("Offset", "Required");
            IActionResult result = await noteController.ListNotes(null);
            Assert.IsInstanceOfType<BadRequestObjectResult>(result);
        }
    }
}
