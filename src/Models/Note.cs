using System;

namespace NoteBin.Models
{
    public class Note
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public DateTime CreationTime { get; init; }
        public string Syntax { get; init; }
        public string? Content { get; set; }

        public Note(string id, string name, DateTime creationTime, string syntaxId, string? content = null)
        {
            Id = id;
            Name = name;
            CreationTime = creationTime;
            Syntax = syntaxId;
            Content = content;
        }
    }
}
