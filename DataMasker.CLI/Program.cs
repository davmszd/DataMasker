using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

namespace DataMasker.CLI
{
    public partial class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var connectionStringOption = new Option<string>(new[] { "-c", "--connection-string" },
                //() => "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=test_db;Pooling=true;",
                () => "",
                "Connection strig which would be use for masking");

            var tableNameOption = new Option<string>(new[] { "-t", "--table-name" },
                () => "",
                "table name that would be queried from the bank");

            var rootCommand = new RootCommand();
            rootCommand.AddOption(connectionStringOption);
            rootCommand.AddOption(tableNameOption);
            rootCommand.SetHandler(Process, connectionStringOption, tableNameOption);

            var parser = new CommandLineBuilder(rootCommand).Build();
            return await parser.InvokeAsync(args);
        }

        public static async Task Process(string connectionString, string tablename)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("connection string cannot be null");
                await Task.CompletedTask;
                return;
            }

            var PGConnectionManager = ConnectionFactory.GetInstance<PGConnectionManager>(connectionString);
            string outPut = string.Empty;
            if (string.IsNullOrEmpty(tablename))
            {
                outPut = await PGConnectionManager.GetTableNamesAsync();
            }
            else
            { 
                outPut = await PGConnectionManager.GetTableColumnNamesAsync(tablename);
            }
            Console.WriteLine(outPut);
        }
    }
}