using System.Data.Common;
using System.Data.SQLite;

namespace NoteBin.Models.Sqlite
{
    public class SelectNotesCmd : DbSelectCmd<Note>
    {
        private readonly string id;

        public SelectNotesCmd(SQLiteConnection connection, string id)
            : base(connection, CreateNoteTableCmd.TableName, CreateNoteTableCmd.Columns)
        {
            this.id = id;
            BuildCommand();
        }

        protected override string BuildFilter()
        {
            cmd.Parameters.AddWithValue("@id", id);
            return $"{CreateNoteTableCmd.IdColumn} = @id";
        }

        protected override Note ParseDataRow(DbDataReader reader)
        {
            long creationTime = reader.GetInt64(1);
            string syntax = reader.GetString(2);
            return new Note(id, TimeUtils.FromUnixTimeMilliseconds(creationTime), syntax);
        }
    }
}
