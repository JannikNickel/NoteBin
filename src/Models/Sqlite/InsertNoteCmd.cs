using System.Data.SQLite;

namespace NoteBin.Models.Sqlite
{
    public class InsertNoteCmd : DbInsertCmd
    {
        public InsertNoteCmd(SQLiteConnection connection, Note note)
            : base(connection, CreateNoteTableCmd.TableName, CreateNoteTableCmd.Columns, GetColumnBindings(note))
        {

        }

        private static object[] GetColumnBindings(Note note) => [note.Id, note.Name, TimeUtils.ToUnixTimeMilliseconds(note.CreationTime), note.Syntax];
    }
}