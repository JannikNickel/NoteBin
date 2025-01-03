using System.ComponentModel.DataAnnotations;

namespace NoteBin.Models.API
{
    public class NoteCreateRequest
    {
        [StringLength(Constants.NoteNameLengthLimit)]
        public string? Name { get; set; }

        [StringLength(Constants.NoteIdLength)]
        public string? Fork { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(Constants.SyntaxLengthLimit)]
        public required string Syntax { get; set; }

        [Required(AllowEmptyStrings = false)]
        public required string Content { get; set; }
    }
}
