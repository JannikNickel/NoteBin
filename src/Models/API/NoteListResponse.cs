using System.Collections.Generic;

namespace NoteBin.Models.API
{
    public record NoteListResponse(List<Note> Notes, long Total);
}