using System;

namespace NoteBin.Models
{
    public class Note
    {
        public string Id { get; init; }
        public DateTime CreationDate { get; init; }
        public string Syntax { get; init; }
        public string? Content { get; set; }

        public Note(string id, DateTime creationDate, string syntaxId, string? content = null)
        {
            Id = id;
            CreationDate = creationDate;
            Syntax = syntaxId;
            Content = content;
        }
    }
}
