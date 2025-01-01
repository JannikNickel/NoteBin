using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NoteBin.Models.API
{
	public record ErrorResponse(string Error)
	{
		public static ObjectResult UsernameDuplicate => WithStatus(StatusCodes.Status409Conflict, "Username already exists!");

        public static ObjectResult WithStatus(int statusCode, string error) => new ObjectResult(new ErrorResponse(error))
		{
			StatusCode = statusCode
		};
	}
}