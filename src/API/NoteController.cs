using Microsoft.AspNetCore.Mvc;
using NoteBin.Models;
using NoteBin.Models.API;
using NoteBin.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoteBin.API
{
    [ApiController]
    [Route($"{Constants.ApiPrefix}/note")]
    public class NoteController : ControllerBase
    {
        private readonly INoteDbService noteService;
        private readonly IAuthService authService;

        public NoteController(INoteDbService noteService, IAuthService authService)
        {
            this.noteService = noteService;
            this.authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] NoteCreateRequest? request)
        {
            if(!ModelState.IsValid || request == null)
            {
                return BadRequest(ModelState);
            }

            string? token = AuthHelper.ReadBearerToken(Request);
            User? owner = token != null ? await authService.ValidateToken(token) : null;
            Note? res = await noteService.SaveNote(request, owner);
            return res != null ? Ok(new NoteCreateResponse(res.Id)) : ErrorResponse.InternalError;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNoteById(string id)
        {
            Note? note = await noteService.GetNote(id);
            return note != null ? Ok(note) : NotFound();
        }

        [HttpGet("list")]
        public async Task<IActionResult> ListNotes([FromQuery] NoteListRequest? request)
        {
            if(!ModelState.IsValid || request == null)
            {
                return BadRequest(ModelState);
            }

            (List<Note> notes, long total) = await noteService.GetNotes(request.Offset, request.Amount, request.Owner, request.Filter);
            return Ok(new NoteListResponse(notes, total));
        }
    }
}
