using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NoteBin.Configuration
{
    public record UserStorageSettings : ISettingsObject
    {
        public static string SectionName => "UserStorage";

        [Required]
        public UserStorageType StorageType { get; init; }
        public string? ConnectionString { get; init; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(StorageType == UserStorageType.SQLite && string.IsNullOrEmpty(ConnectionString))
            {
                yield return new ValidationResult($"{nameof(ConnectionString)} is required if {nameof(StorageType)} is {UserStorageType.SQLite}!");
            }
        }
    }
}
