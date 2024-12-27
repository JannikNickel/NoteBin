namespace NoteBin.Configuration
{
    public record NoteStorageSettings
    {
        public static string SectionName => "NoteStorage";

        public NoteStorageType StorageType { get; init; }
        public string? ConnectionString { get; init; }
    }
}
