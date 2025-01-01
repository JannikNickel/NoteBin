using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NoteBin.Configuration
{
    public record AuthSettings : ISettingsObject
    {
        public static string SectionName => "AuthSettings";

        [Required]
        public AuthType AuthType { get; init; }
        [Required]
        public required string KeyFile { get; init; }
        [Required, Range(64, 256)]
        public int KeyLength { get; init; }
        [Required, Range(64, 256)]
        public int TokenLength { get; init; }
        [Required]
        public int ExpirationDuration { get; init; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
}
