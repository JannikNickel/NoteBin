using System.ComponentModel.DataAnnotations;

namespace NoteBin.Configuration
{
    public interface ISettingsObject : IValidatableObject
    {
        static abstract string SectionName { get; }
    }
}
