using Microsoft.AspNetCore.Mvc;
using NoteBin.Models;
using NoteBin.Models.API;
using NoteBin.Services;
using System.Threading.Tasks;

namespace NoteBin.API
{
    [ApiController]
    [Route($"{Constants.ApiPrefix}/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserDbService dbService;

        public UserController(IUserDbService dbService)
        {
            this.dbService = dbService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserRequest? request)
        {
            if(!ModelState.IsValid || request == null)
            {
                return BadRequest(ModelState);
            }

            User? res = await dbService.CreateUser(request);
            return res != null ? Ok() : ErrorResponse.UsernameDuplicate;
        }
    }
}