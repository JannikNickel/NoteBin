using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NoteBin.Models.API
{
    public record ErrorResponse(string Error)
    {
        public static ObjectResult InternalError => WithStatus(StatusCodes.Status500InternalServerError, "An unexpected error occurred!");
        public static ObjectResult DuplicateUsername => WithStatus(StatusCodes.Status409Conflict, "Username already exists!");
        public static ObjectResult InvalidUsername => WithStatus(StatusCodes.Status400BadRequest, $"Invalid username! (length <= {Constants.UserNameLengthLimit}, allowed characters: a-z, A-Z, 0-9, _)");
        public static ObjectResult InvalidPassword => WithStatus(StatusCodes.Status400BadRequest, $"Invalid password! (length >= {Constants.PasswordMinLength}, must contain lowercase letter, uppercase letter and digit");

        public static ObjectResult WithStatus(int statusCode, string error) => new ObjectResult(new ErrorResponse(error))
        {
            StatusCode = statusCode
        };
    }
}