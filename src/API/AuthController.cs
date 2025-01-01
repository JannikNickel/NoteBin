using Microsoft.AspNetCore.Mvc;
using NoteBin.Models;
using NoteBin.Models.API;
using NoteBin.Services;
using System.Threading.Tasks;

namespace NoteBin.API
{
    [ApiController]
    [Route($"{Constants.ApiPrefix}/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserDbService userService;
        private readonly IAuthService authService;

        public AuthController(IUserDbService dbService, IAuthService authService)
        {
            this.userService = dbService;
            this.authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserRequest? request)
        {
            if(!ModelState.IsValid || request == null)
            {
                return BadRequest(ModelState);
            }

            User? user = await userService.GetUser(request.Username ?? "");
            if(user != null)
            {
                bool valid = userService.VerifyUser(user, request.Password ?? "");
                if(valid)
                {
                    string token = await authService.GenerateToken(user);
                    return Ok(new AuthResponse(token));
                }
            }
            return Unauthorized();
        }

        [HttpGet]
        public async Task<IActionResult> Validate()
        {
            string? token = AuthHelper.ReadBearerToken(Request);
            if(token == null)
            {
                return BadRequest();
            }

            bool valid = await authService.ValidateToken(token) != null;
            return valid ? base.Ok() : base.Unauthorized();
        }
    }
}
