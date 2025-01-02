using System.ComponentModel.DataAnnotations;

namespace NoteBin.Models.API
{
    public class AuthRequest
    {
        [Required(AllowEmptyStrings = true, ErrorMessage = $"{nameof(Username)} is required!")]
        [StringLength(32, ErrorMessage = $"{nameof(Username)} must not exceed 32 characters")]
        public required string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = $"{nameof(Password)} is required!")]
        [StringLength(32, ErrorMessage = $"{nameof(Password)} must not exceed 32 characters")]
        public required string Password { get; set; }
    }
}
