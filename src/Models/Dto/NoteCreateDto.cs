using System.ComponentModel.DataAnnotations;

namespace NoteBin.Models.Dto
{
    public class NoteCreateDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Syntax is required!")]
        [StringLength(32, ErrorMessage = "Syntax must not exceed 32 characters")]
        public string? Syntax { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Content is required!")]
        public string? Content { get; set; }
    }
}
