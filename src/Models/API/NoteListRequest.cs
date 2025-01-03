using System.ComponentModel.DataAnnotations;

namespace NoteBin.Models.API
{
    public class NoteListRequest
    {
        [Required, Range(0L, long.MaxValue)]
        public long Offset { get; set; }

        [Required, Range(0L, Constants.NotePageSizeLimit)]
        public long Amount { get; set; }

        [StringLength(Constants.UserNameLengthLimit)]
        public string? Owner { get; set; }

        [StringLength(Constants.NoteNameLengthLimit)]
        public string? Filter { get; set; }
    }
}
