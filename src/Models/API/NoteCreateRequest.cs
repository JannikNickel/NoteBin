using System.ComponentModel.DataAnnotations;

namespace NoteBin.Models.API
{
    public class NoteCreateRequest
    {
        [StringLength(64, ErrorMessage = $"{nameof(Name)} must not exceed 64 characters")]
        public string? Name { get; set; }

        [StringLength(64, ErrorMessage = $"{nameof(Fork)} must not exceed 64 characters")]
        public string? Fork { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = $"{nameof(Syntax)} is required!")]
        [StringLength(32, ErrorMessage = $"{nameof(Syntax)} must not exceed 32 characters")]
        public required string Syntax { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = $"{nameof(Content)} is required!")]
        public required string Content { get; set; }
    }
}
