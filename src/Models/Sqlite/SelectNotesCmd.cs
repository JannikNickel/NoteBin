using System.Data.Common;
using System.Data.SQLite;

namespace NoteBin.Models.Sqlite
{
    public class SelectNotesCmd : DbSelectCmd<Note>
    {
        private readonly long offset;
        private readonly long limit;
        private readonly string? user;
        private readonly NoteSortOrder sortOrder;

        public SelectNotesCmd(SQLiteConnection connection, long offset, long limit, string? user = null, NoteSortOrder sortOrder = NoteSortOrder.None)
            : base(connection, CreateNoteTableCmd.TableName, CreateNoteTableCmd.Columns)
        {
            this.offset = offset;
            this.limit = limit;
            this.user = user;
            this.sortOrder = sortOrder;
            BuildCommand();
        }

        protected override string BuildOrder() => sortOrder switch
        {
            NoteSortOrder.CreationTimeDesc => $"ORDER BY {CreateNoteTableCmd.CreationTimeColumn} DESC",
            _ => ""
        };

        protected override string BuildFilter()
        {
            if(user != null)
            {
                cmd.Parameters.AddWithValue("@user", user);
                return $"WHERE {CreateNoteTableCmd.OwnerColumn} = @user";
            }
            return "";
        }

        protected override string BuildOther()
        {
            cmd.Parameters.AddWithValue("@limit", limit);
            cmd.Parameters.AddWithValue("@offset", offset);
            return $"LIMIT @limit OFFSET @offset";
        }

        protected override Note ParseDataRow(DbDataReader reader) => CreateNoteTableCmd.ParseNoteFromRow(reader);
    }
}
