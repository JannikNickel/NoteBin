using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading.Tasks;

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
        public const string CreationTimeIndex = "idx_creation_time";

        public static string[] Columns => [IdColumn, NameColumn, OwnerColumn, ForkColumn, CreationTimeColumn, SyntaxColumn];

        public CreateNoteTableCmd(SQLiteConnection connection) : base(connection, TableName)
        {

        }

        protected override IEnumerable<string> GenerateColumns()
        {
            yield return $"{IdColumn} TEXT PRIMARY KEY";
            yield return $"{NameColumn} TEXT NOT NULL";
            yield return $"{OwnerColumn} TEXT";
            yield return $"{ForkColumn} TEXT";
            yield return $"{CreationTimeColumn} INTEGER NOT NULL";
            yield return $"{SyntaxColumn} TEXT NOT NULL";
        }

        public void Execute()
        {
            cmd.ExecuteNonQuery();

            //Add index on creation_time column
            base.NextCommand();
            cmd.CommandText = $"CREATE INDEX IF NOT EXISTS {CreationTimeIndex} ON {tableName}({CreationTimeColumn})";
            cmd.ExecuteNonQuery();
        }
    }
}
