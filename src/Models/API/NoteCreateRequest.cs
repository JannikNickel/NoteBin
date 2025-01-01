using System.ComponentModel.DataAnnotations;

namespace NoteBin.Models.API
{
    public class NoteCreateRequest
    {
        [Required(AllowEmptyStrings = true, ErrorMessage = $"{nameof(Name)} is required!")]
        [StringLength(64, ErrorMessage = $"{nameof(Name)} must not exceed 64 characters")]
        public string? Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = $"{nameof(Syntax)} is required!")]
        [StringLength(32, ErrorMessage = $"{nameof(Syntax)} must not exceed 32 characters")]
        public string? Syntax { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = $"{nameof(Content)} is required!")]
        public string? Content { get; set; }
    }
}
