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
        public async Task<IActionResult> CreateUser([FromBody] AuthRequest? request)
        {
            if(!ModelState.IsValid || request == null)
            {
                return BadRequest(ModelState);
            }

            UserCreationResult res = await dbService.CreateUser(request);
            return res.IsOk ? Ok() : res.Error switch
            {
                UserCreationError.InvalidUsername => ErrorResponse.InvalidUsername,
                UserCreationError.InvalidPassword => ErrorResponse.InvalidPassword,
                UserCreationError.DuplicateUsername => ErrorResponse.DuplicateUsername,
                _ => ErrorResponse.InternalError
            };
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetUserByName(string name)
        {
            User? user = await dbService.GetUser(name);
            return user != null ? Ok(UserResponse.FromUser(user)) : NotFound();
        }
    }
}
