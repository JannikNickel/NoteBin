namespace NoteBin
{
    public static class Constants
    {
        public const string ApiPrefix = "/api";
        public const string WebDir = "web";

        public const int UserNameLengthLimit = 32;
        public const int PasswordLengthLimit = 32;
        public const int PasswordMinLength = 8;

        public const int NoteIdLength = 10;
        public const int NoteNameLengthLimit = 32;
        public const int SyntaxLengthLimit = 32;

        public const long NotePageSizeLimit = 100;
        public const int NotePreviewLength = 256;
    }
}
