using System.ComponentModel.DataAnnotations;

namespace NoteBin.Models.API
{
    public class AuthRequest
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(Constants.UserNameLengthLimit)]
        public required string Username { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(Constants.PasswordLengthLimit)]
        public required string Password { get; set; }
    }
}
