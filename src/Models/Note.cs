using System;

namespace NoteBin.Models
{
    public class Note
    {
        public string Id { get; init; }
        public DateTime CreationDate { get; init; }
        public string Syntax { get; init; }
        public string Content { get; init; }

        public Note(string id, DateTime creationDate, string syntaxId, string content)
        {
            Id = id;
            CreationDate = creationDate;
            Syntax = syntaxId;
            Content = content;
        }
    }
}
