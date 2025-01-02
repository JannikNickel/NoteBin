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
            string name = reader.GetString(1);
            string? owner = reader.GetValue(2) as string;
            string? fork = reader.GetValue(3) as string;
            long creationTime = reader.GetInt64(4);
            string syntax = reader.GetString(5);
            return new Note(id, name, owner, fork, TimeUtils.FromUnixTimeMilliseconds(creationTime), syntax);
        }
    }
}
