using System.ComponentModel.DataAnnotations;

namespace NoteBin.Models.API
{
    public class NoteListRequest
    {
        [Required, Range(0L, long.MaxValue)]
        public long Offset { get; set; }

        [Required, Range(0, 100)]
        public long Amount { get; set; }

        [StringLength(32, ErrorMessage = $"{nameof(Owner)} must not exceed 32 characters")]
        public string? Owner { get; set; }
    }
}
