using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoteBin.Models;
using NoteBin.Models.API;
using NoteBin.Services;
using System.Threading.Tasks;

namespace NoteBin.API
{
    [ApiController]
    [Route($"{Constants.ApiPrefix}/note")]
    public class NoteController : ControllerBase
    {
        private readonly INoteDbService dbService;

        public NoteController(INoteDbService dbService)
        {
            this.dbService = dbService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] NoteCreateRequest? request)
        {
            if(!ModelState.IsValid || request == null)
            {
                return BadRequest(ModelState);
            }

            Note? res = await dbService.SaveNote(request);
            return res != null ? Ok(new { res.Id }) : StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNoteById(string id)
        {
            Note? note = await dbService.GetNote(id);
            return note != null ? Ok(note) : NotFound();
        }

        //TODO api for listing items
        //Paged (offset, amount with limit)
        //Returns the total amount of available items
        //Sorted by date?
    }
}
