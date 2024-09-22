

namespace DBEntityGenerator
{
    class Program
    {
        static void Main(string[] args)
        {

            string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Northwind;Trusted_Connection=True;MultipleActiveResultSets=true;"; // Set your DB connection string

            string currentDirectory = Directory.GetCurrentDirectory();


            string directory = Directory.GetParent(Directory.GetParent(Directory.GetParent(currentDirectory).FullName).FullName).FullName + "\\Models";

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }


            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Retrieve all table and column information using a single query
                var tableColumns = GetTableColumns(connection);

                // Group the result by table names and generate Fluent API for OnModelCreating
                var groupedByTables = tableColumns.AsEnumerable()
                                                  .GroupBy(row => row["TABLE_NAME"].ToString());

                GenerateOnModelCreating(directory,groupedByTables);
            }
        }

        // Method to get all table names and column information in one query
        static DataTable GetTableColumns(SqlConnection connection)
        {
            string query = @"
                SELECT 
                    t.TABLE_NAME, 
                    c.COLUMN_NAME, 
                    c.DATA_TYPE, 
                    c.IS_NULLABLE,
                    c.CHARACTER_MAXIMUM_LENGTH
                FROM 
                    INFORMATION_SCHEMA.TABLES t
                INNER JOIN 
                    INFORMATION_SCHEMA.COLUMNS c 
                ON 
                    t.TABLE_NAME = c.TABLE_NAME
                WHERE 
                    t.TABLE_TYPE = 'BASE TABLE'
                ORDER BY 
                    t.TABLE_NAME, c.ORDINAL_POSITION";

            using (var command = new SqlCommand(query, connection))
            {
                var dataTable = new DataTable();
                using (var reader = command.ExecuteReader())
                {
                    dataTable.Load(reader);
                }
                return dataTable;
            }
        }

        // Method to generate POCO class for a given table
        static void GeneratePocoClass(string directory, string tableName, IEnumerable<DataRow> columns)
        {
            //var className = ToPascalCase(tableName);
            var className = tableName;
            var sb = new StringBuilder();

            sb.AppendLine();
            sb.AppendLine($"namespace DBClassGenerator;");
            sb.AppendLine($"public class {className}");
            sb.AppendLine("{");

            foreach (var column in columns)
            {
                var columnName = ToPascalCase(column["COLUMN_NAME"].ToString());
                var dataType = GetClrType(column["DATA_TYPE"].ToString());
                var isNullable = column["IS_NULLABLE"].ToString() == "YES";

                // Add the property with a nullable type if column allows null
                sb.AppendLine($"\tpublic {(isNullable && dataType != "string" ? dataType + "?" : dataType)} {columnName} {{ get; set; }}");
            }

            sb.AppendLine("}");


            var filePath = Path.Combine(directory, $"{className}.cs");
            File.WriteAllText(filePath, sb.ToString());

            Console.WriteLine($"Generated: {className}.cs");
        }

        // Method to generate Fluent API calls for OnModelCreating in DbContext
        static void GenerateOnModelCreating(string directory, IEnumerable<IGrouping<string, DataRow>> groupedByTables)
        {
            var sb = new StringBuilder();

            //sb.AppendLine("protected override void OnModelCreating(ModelBuilder modelBuilder)");
            //sb.AppendLine("{");

            foreach (var tableGroup in groupedByTables)
            {
                var tableName = tableGroup.Key; // ToPascalCase(tableGroup.Key);
                sb.AppendLine($"namespace DBEntityGenerator.Entity.Models;");

                sb.AppendLine($"\tpublic class {tableName}Configuration : IEntityTypeConfiguration<{tableName}>");
                sb.AppendLine("\t{");

                //  public void Configure(EntityTypeBuilder<DB.Product> entity)
                sb.AppendLine($"\tpublic void Configure(EntityTypeBuilder<{tableName}> entity)");
                sb.AppendLine("\t\t{");

                foreach (var column in tableGroup)
                {
                    var columnName = column["COLUMN_NAME"].ToString();
                    var sqlType = column["DATA_TYPE"].ToString();
                    var isNullable = column["IS_NULLABLE"].ToString() == "YES";
                    var maxLength = column["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value ? column["CHARACTER_MAXIMUM_LENGTH"].ToString() : null;

                    // Set column type and constraints
                    sb.Append($"\t\t\tentity.Property(e => e.{columnName})");

                    // Define the column type mapping
                    if (sqlType.ToLower() == "nvarchar" || sqlType.ToLower() == "varchar" || sqlType.ToLower() == "char")
                    {
                        if (maxLength != null)
                        {
                            sb.Append($".HasMaxLength({maxLength})");
                        }
                    }

                    sb.Append($".HasColumnType(\"{sqlType}\")");

                    // Handle nullable columns
                    if (!isNullable)
                    {
                        sb.Append(".IsRequired()");
                    }

                    sb.AppendLine(";");
                }

                sb.AppendLine("\t\t}");
                sb.AppendLine("\t}");

                //sb.AppendLine("}");

                // Output the generated code to a file
                var filePath = Path.Combine(directory, $"{tableName}.cs");
                File.WriteAllText(filePath, sb.ToString());
                Console.WriteLine($"public virtual DbSet<{tableName}> {tableName} {{ get; set; }}");
            }

            //sb.AppendLine("}");



            Console.WriteLine("Generated: OnModelCreating.cs");
        }


        static string GetClrType(string sqlType)
        {
            switch (sqlType.ToLower())
            {
                case "int": return "int";
                case "bigint": return "long";
                case "smallint": return "short";
                case "tinyint": return "byte";
                case "bit": return "bool";
                case "decimal":
                case "numeric": return "decimal";
                case "float": return "double";
                case "real": return "float";
                case "datetime":
                case "smalldatetime":
                case "date":
                case "time": return "DateTime";
                case "char":
                case "nchar":
                case "nvarchar":
                case "varchar": return "string";
                case "uniqueidentifier": return "Guid";
                case "varbinary":
                case "binary": return "byte[]";
                case "ntext": return "string";
                case "image": return "byte[]";
                case "money": return "decimal";
                default: return "object";
            }
        }

        // Method to convert table/column names to PascalCase
        static string ToPascalCase(string input)
        {
            return input;
            //return string.Join("", input.Split(new[] { '_', ' ' }, StringSplitOptions.RemoveEmptyEntries)
            //                            .Select(word => char.ToUpper(word[0]) + word.Substring(1).ToLower()));
        }
    }
}
