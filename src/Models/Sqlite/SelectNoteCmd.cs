using System.Data.Common;
using System.Data.SQLite;

namespace NoteBin.Models.Sqlite
{
    public class SelectNoteCmd : DbSelectCmd<Note>
    {
        private readonly string id;

        public SelectNoteCmd(SQLiteConnection connection, string id)
            : base(connection, CreateNoteTableCmd.TableName, CreateNoteTableCmd.Columns)
        {
            this.id = id;
            BuildCommand();
        }

        protected override string BuildFilter()
        {
            cmd.Parameters.AddWithValue("@id", id);
            return $"WHERE {CreateNoteTableCmd.IdColumn} = @id";
        }

        protected override Note ParseDataRow(DbDataReader reader) => CreateNoteTableCmd.ParseNoteFromRow(reader);
    }
}
