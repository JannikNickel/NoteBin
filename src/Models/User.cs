using System;

namespace NoteBin.Models
{
    public class User
    {
        public string Name { get; init; }
        public string Password { get; init; }
        public DateTime CreationTime { get; init; }

        public User(string name, string password, DateTime creationTime)
        {
            Name = name;
            Password = password;
            CreationTime = creationTime;
        }
    }
}
