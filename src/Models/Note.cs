using System;

namespace NoteBin.Models
{
    public class Note
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string? Owner { get; init; }
        public DateTime CreationTime { get; init; }
        public string Syntax { get; init; }
        public string? Content { get; set; }

        public Note(string id, string name, string? owner, DateTime creationTime, string syntaxId, string? content = null)
        {
            Id = id;
            Name = name;
            Owner = owner;
            CreationTime = creationTime;
            Syntax = syntaxId;
            Content = content;
        }
    }
}
