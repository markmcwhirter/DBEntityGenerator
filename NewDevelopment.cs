

//using System.Data;

//namespace DBEntityGenerator
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {

//            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Northwind;Trusted_Connection=True;MultipleActiveResultSets=true;"; // Set your DB connection string

//            string currentDirectory = Directory.GetCurrentDirectory();


//            string directory = "C:\\temp";   // Directory.GetParent(Directory.GetParent(Directory.GetParent(currentDirectory).FullName).FullName).FullName + "\\Models";

//            if (!Directory.Exists(directory))
//            {
//                Directory.CreateDirectory(directory);
//            }


//            using (var connection = new SqlConnection(connectionString))
//            {
//                connection.Open();

//                // Retrieve all table and column information using a single query
//                DataTable tableColumns = GetTableColumns(connection);
//                var tableIndexes = GetTableIndexes(connection);
//                var tableForeignKeys = GetForeignKeys(connection);


//                // Group the result by table names and generate Fluent API for OnModelCreating
//                var groupedByTables = tableColumns.AsEnumerable()
//                                                  .GroupBy(row => row["TABLE_NAME"].ToString());

//                GenerateOnModelCreating(directory, tableColumns, tableIndexes, tableForeignKeys);
//            }
//        }
//        // Method to get table index information
//        static DataTable GetTableIndexes(SqlConnection connection)
//        {
//            string query = @"
//                SELECT 
//                    i.name AS INDEX_NAME,
//                    t.name AS TABLE_NAME,
//                    c.name AS COLUMN_NAME,
//                    i.is_unique,
//                    i.is_primary_key
//                FROM 
//                    sys.indexes i
//                INNER JOIN 
//                    sys.index_columns ic ON i.index_id = ic.index_id
//                INNER JOIN 
//                    sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
//                INNER JOIN 
//                    sys.tables t ON t.object_id = c.object_id
//                WHERE 
//                    i.is_primary_key = 0 AND i.is_unique_constraint = 0
//                ORDER BY 
//                    t.name, i.name, ic.index_column_id";

//            using (var command = new SqlCommand(query, connection))
//            {
//                var dataTable = new DataTable();
//                using (var reader = command.ExecuteReader())
//                {
//                    dataTable.Load(reader);
//                }
//                return dataTable;
//            }
//        }

//        // Method to get foreign key information
//        static DataTable GetForeignKeys(SqlConnection connection)
//        {
//            string query = @"
//                SELECT 
//                    fk.CONSTRAINT_NAME, 
//                    fk.TABLE_NAME AS TABLE_NAME,
//                    fk.COLUMN_NAME AS FK_COLUMN_NAME, 
//                    pk.TABLE_NAME AS REFERENCED_TABLE_NAME, 
//                    pk.COLUMN_NAME AS REFERENCED_COLUMN_NAME
//                FROM 
//                    INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc
//                JOIN 
//                    INFORMATION_SCHEMA.KEY_COLUMN_USAGE fk 
//                    ON rc.CONSTRAINT_NAME = fk.CONSTRAINT_NAME
//                JOIN 
//                    INFORMATION_SCHEMA.KEY_COLUMN_USAGE pk 
//                    ON rc.UNIQUE_CONSTRAINT_NAME = pk.CONSTRAINT_NAME
//                WHERE 
//                    fk.ORDINAL_POSITION IS NOT NULL";

//            using (var command = new SqlCommand(query, connection))
//            {
//                var dataTable = new DataTable();
//                using (var reader = command.ExecuteReader())
//                {
//                    dataTable.Load(reader);
//                }
//                return dataTable;
//            }
//        }
//        // Method to get all table names and column information in one query
//        static DataTable GetTableColumns(SqlConnection connection)
//        {
//            string query = @"SELECT
//                    T.TABLE_NAME AS TableName,
//                    C.COLUMN_NAME AS ColumnName,
//                    C.ORDINAL_POSITION AS ColumnOrder,
//                    C.DATA_TYPE AS ColumnDataType,
//                    C.CHARACTER_MAXIMUM_LENGTH AS MaxLength,
//                    C.NUMERIC_PRECISION AS NumericPrecision,
//                    C.NUMERIC_SCALE AS NumericScale,
//                    C.IS_NULLABLE AS IsNullable
//                FROM
//                    INFORMATION_SCHEMA.TABLES T
//                INNER JOIN
//                    INFORMATION_SCHEMA.COLUMNS C
//                ON
//                    T.TABLE_CATALOG = C.TABLE_CATALOG
//                    AND T.TABLE_SCHEMA = C.TABLE_SCHEMA
//                    AND T.TABLE_NAME = C.TABLE_NAME
//                WHERE
//                    T.TABLE_TYPE = 'BASE TABLE'-- Exclude views, etc.
//                ORDER BY
//                    T.TABLE_CATALOG, T.TABLE_SCHEMA, T.TABLE_NAME, C.ORDINAL_POSITION;";

//            using (var command = new SqlCommand(query, connection))
//            {
//                var dataTable = new DataTable();
//                using (var reader = command.ExecuteReader())
//                {
//                    dataTable.Load(reader);
//                }
//                return dataTable;
//            }
//        }

//        // Method to generate POCO class for a given table
//        static void GeneratePocoClass(string directory, string tableName, IEnumerable<DataRow> columns)
//        {
//            //var className = ToPascalCase(tableName);
//            var className = tableName;
//            var sb = new StringBuilder();

//            sb.AppendLine();
//            sb.AppendLine($"namespace DBClassGenerator;");
//            sb.AppendLine($"public class {className}");
//            sb.AppendLine("{");

//            foreach (var column in columns)
//            {
//                var columnName = ToPascalCase(column["COLUMN_NAME"].ToString());
//                var dataType = GetClrType(column["DATA_TYPE"].ToString());
//                var isNullable = column["IS_NULLABLE"].ToString() == "YES";

//                // Add the property with a nullable type if column allows null
//                sb.AppendLine($"\tpublic {(isNullable && dataType != "string" ? dataType + "?" : dataType)} {columnName} {{ get; set; }}");
//            }

//            sb.AppendLine("}");


//            var filePath = Path.Combine(directory, $"{className}.cs");
//            File.WriteAllText(filePath, sb.ToString());

//            Console.WriteLine($"Generated: {className}.cs");
//        }

//        // Method to generate Fluent API calls for OnModelCreating in DbContext
//        static void GenerateOnModelCreating(string directory, DataTable groupedByTables, DataTable tableIndexes, DataTable tableForeignKeys)
//        {
//            var sb = new StringBuilder();

//            foreach (DataRow row in groupedByTables.Rows)
//            {

//                var tableName = groupedByTables.Rows[0][0].ToString(); // ToPascalCase(tableGroup.Key);
//                var newtable = tableName;

//                sb.AppendLine($"namespace DBEntityGenerator.Entity.Models;");

//                sb.AppendLine($"\tpublic class {tableName}Configuration : IEntityTypeConfiguration<{tableName}>");
//                sb.AppendLine("\t{");

//                //  public void Configure(EntityTypeBuilder<DB.Product> entity)
//                sb.AppendLine($"\tpublic void Configure(EntityTypeBuilder<{tableName}> entity)");
//                sb.AppendLine("\t\t{");


//                if (newtable != row[0].ToString())
//                {
//                    newtable = row[0].ToString();
//                    sb.AppendLine($"\t\tentity.ToTable(\"{newtable}\", \"dbo\");");
//                    continue;
//                }

//                for (int i = 0; i < row.ItemArray.Length; i++)
//                {
//                    //Console.Write($"{column.ColumnName}: {row[i]} | ");


//                    var columnName = row[0].ToString();  // "COLUMN_NAME"
//                    var sqlType = row[3].ToString(); // "DATA_TYPE"
//                    var isNullable = row[7].ToString() == "YES";
//                    var maxLength = row[4] != DBNull.Value ? row[4].ToString() : null;

//                    // Set column type and constraints
//                    sb.Append($"\t\t\tentity.Property(e => e.{columnName})");

//                    // Define the column type mapping
//                    if (sqlType.ToLower() == "nvarchar" || sqlType.ToLower() == "varchar" || sqlType.ToLower() == "char")
//                    {
//                        if (maxLength != null)
//                        {
//                            sb.Append($".HasMaxLength({maxLength})");
//                        }
//                    }

//                    sb.Append($".HasColumnType(\"{sqlType}\")");

//                    // Handle nullable columns
//                    if (!isNullable)
//                    {
//                        sb.Append(".IsRequired()");
//                    }

//                    sb.AppendLine(";");
//                }

//                sb.AppendLine("\t\t}");
//                sb.AppendLine("\t}");

//                //sb.AppendLine("}");

//                // Generate index definitions
//                // TODO
//                //var tableIndexRows = tableIndexes.AsEnumerable().Where(row => row["TABLE_NAME"].ToString() == row.Key);
//                //foreach (var indexRow in tableIndexRows)
//                //{
//                //    var indexName = indexRow["INDEX_NAME"].ToString();
//                //    var columnName = ToPascalCase(indexRow["COLUMN_NAME"].ToString());
//                //    var isUnique = indexRow["is_unique"].ToString() == "True";

//                //    sb.AppendLine($"\t\tentity.HasIndex(e => e.{columnName})");

//                //    if (isUnique)
//                //    {
//                //        sb.AppendLine($"\t\t\t.IsUnique()");
//                //    }

//                //    sb.AppendLine($"\t\t\t.HasDatabaseName(\"{indexName}\");");
//                //}

//                // Generate foreign key definitions
//                //var foreignKeyRows = tableForeignKeys.AsEnumerable().Where(row => row["TABLE_NAME"].ToString() == row.Key);
//                //foreach (var fkRow in foreignKeyRows)
//                //{
//                //    var fkColumnName = ToPascalCase(fkRow["FK_COLUMN_NAME"].ToString());
//                //    var referencedTableName = ToPascalCase(fkRow["REFERENCED_TABLE_NAME"].ToString());
//                //    var referencedColumnName = ToPascalCase(fkRow["REFERENCED_COLUMN_NAME"].ToString());

//                //    sb.AppendLine($"\t\tentity.HasOne(d => d.{referencedTableName})");
//                //    sb.AppendLine($"\t\t\t.WithMany(p => p.{tableName}s)");  // Assuming convention for navigation properties
//                //    sb.AppendLine($"\t\t\t.HasForeignKey(d => d.{fkColumnName})");
//                //    sb.AppendLine($"\t\t\t.HasConstraintName(\"{fkRow["CONSTRAINT_NAME"]}\");");
//                //}
//            }
//            // Output the generated code to a file
//            var filePath = Path.Combine(directory, $"{tableName}.cs");
//            File.WriteAllText(filePath, sb.ToString());
//            Console.WriteLine($"public virtual DbSet<{tableName}> {tableName} {{ get; set; }}");



//            //sb.AppendLine("}");



//            Console.WriteLine("Generated: OnModelCreating.cs");
//        }


//        static string GetClrType(string sqlType)
//        {
//            switch (sqlType.ToLower())
//            {
//                case "int": return "int";
//                case "bigint": return "long";
//                case "smallint": return "short";
//                case "tinyint": return "byte";
//                case "bit": return "bool";
//                case "decimal":
//                case "numeric": return "decimal";
//                case "float": return "double";
//                case "real": return "float";
//                case "datetime":
//                case "smalldatetime":
//                case "date":
//                case "time": return "DateTime";
//                case "char":
//                case "nchar":
//                case "nvarchar":
//                case "varchar": return "string";
//                case "uniqueidentifier": return "Guid";
//                case "varbinary":
//                case "binary": return "byte[]";
//                case "ntext": return "string";
//                case "image": return "byte[]";
//                case "money": return "decimal";
//                default: return "object";
//            }
//        }

//        // Method to convert table/column names to PascalCase
//        static string ToPascalCase(string input)
//        {
//            return input;
//            //return string.Join("", input.Split(new[] { '_', ' ' }, StringSplitOptions.RemoveEmptyEntries)
//            //                            .Select(word => char.ToUpper(word[0]) + word.Substring(1).ToLower()));
//        }
//    }
//}
