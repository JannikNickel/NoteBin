using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;

namespace NoteBin.Models.Sqlite
{
    public class CreateNoteTableCmd : DbCreateTableCmd
    {
        public const string TableName = "notes";
        public const string IdColumn = "id";
        public const string NameColumn = "name";
        public const string OwnerColumn = "owner";
        public const string ForkColumn = "fork";
        public const string CreationTimeColumn = "creation_time";
        public const string SyntaxColumn = "syntax";

        public const string NameIndex = "idx_name";
        public const string OwnerIndex = "idx_owner";
        public const string CreationTimeIndex = "idx_creation_time";

        public static string[] Columns => [IdColumn, NameColumn, OwnerColumn, ForkColumn, CreationTimeColumn, SyntaxColumn];

        public CreateNoteTableCmd(SQLiteConnection connection) : base(connection, TableName) { }

        protected override IEnumerable<string> GenerateColumns()
        {
            yield return $"{IdColumn} TEXT PRIMARY KEY";
            yield return $"{NameColumn} TEXT";
            yield return $"{OwnerColumn} TEXT";
            yield return $"{ForkColumn} TEXT";
            yield return $"{CreationTimeColumn} INTEGER NOT NULL";
            yield return $"{SyntaxColumn} TEXT NOT NULL";
        }

        public void Execute()
        {
            cmd.ExecuteNonQuery();

            //Add index to name column
            base.NextCommand();
            cmd.CommandText = $"CREATE INDEX IF NOT EXISTS {NameIndex} ON {tableName}({NameColumn})";
            cmd.ExecuteNonQuery();

            //Add index to owner column
            base.NextCommand();
            cmd.CommandText = $"CREATE INDEX IF NOT EXISTS {OwnerIndex} ON {tableName}({OwnerColumn})";
            cmd.ExecuteNonQuery();

            //Add index on creation_time column
            base.NextCommand();
            cmd.CommandText = $"CREATE INDEX IF NOT EXISTS {CreationTimeIndex} ON {tableName}({CreationTimeColumn})";
            cmd.ExecuteNonQuery();
        }

        public static Note ParseNoteFromRow(DbDataReader reader)
        {
            string id = reader.GetString(0);
            string? name = reader.GetValue(1) as string;
            string? owner = reader.GetValue(2) as string;
            string? fork = reader.GetValue(3) as string;
            long creationTime = reader.GetInt64(4);
            string syntax = reader.GetString(5);
            return new Note(id, name, owner, fork, TimeUtils.FromUnixTimeMilliseconds(creationTime), syntax);
        }
    }
}
