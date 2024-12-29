using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NoteBin.Configuration
{
    public record NoteStorageSettings : IValidatableObject
    {
        public static string SectionName => "NoteStorage";

        [Required]
        public NoteStorageType StorageType { get; init; }
        public string? ConnectionString { get; init; }
        public string? ContentPath { get; init; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(StorageType == NoteStorageType.SQLite && string.IsNullOrEmpty(ConnectionString))
            {
                yield return new ValidationResult($"{nameof(ConnectionString)} is required if {nameof(StorageType)} is {NoteStorageType.SQLite}!");
            }
            if(StorageType == NoteStorageType.SQLite && string.IsNullOrEmpty(ContentPath))
            {
                yield return new ValidationResult($"{nameof(ContentPath)} is required if {nameof(StorageType)} is {NoteStorageType.SQLite}!");
            }
        }
    }
}
