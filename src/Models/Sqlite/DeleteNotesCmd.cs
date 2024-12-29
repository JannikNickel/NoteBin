using System.Data.Common;
using System.Data.SQLite;

namespace NoteBin.Models.Sqlite
{
    public class DeleteNotesCmd : DbDeleteCmd
    {
        private readonly string id;

        public DeleteNotesCmd(SQLiteConnection connection, string id)
            : base(connection, CreateNoteTableCmd.TableName)
        {
            this.id = id;
            BuildCommand();
        }

        protected override string BuildFilter()
        {
            cmd.Parameters.AddWithValue("@id", id);
            return $"{CreateNoteTableCmd.IdColumn} = @id";
        }
    }
}
