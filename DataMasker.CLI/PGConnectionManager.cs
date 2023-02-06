using Npgsql;
using System.Text;

namespace DataMasker.CLI;

public sealed class PGConnectionManager : IConnectionManager
{
    private readonly string _connectionString;

    public PGConnectionManager(string connectionString)
    {
        this._connectionString = connectionString;
    }

    public async Task<string> GetTableNamesAsync()
    {
        var tableName = new StringBuilder();
        using var DbConnectoin = new NpgsqlConnection(_connectionString);
        DbConnectoin.Open();

        using var pgCommand = new NpgsqlCommand($@"SELECT table_name FROM information_schema.tables WHERE table_schema = 'public';", DbConnectoin);
        using var reader = pgCommand.ExecuteReader();
        while (reader.Read())
        {
            tableName.Append($"{reader["table_name"].ToString()},");
        }

        reader.Close();
        pgCommand.Dispose();
        DbConnectoin.Close();

        return await Task.FromResult(string.Join(Environment.NewLine,
                                                    tableName.
                                                    ToString().
                                                    Split(',')
                                                    .Distinct()
                                                    .Where(x => !string.IsNullOrEmpty(x))
                                                        .ToList()));
    }

    public async Task<string> GetTableColumnNamesAsync(string tableName)
    {
        var builder = new StringBuilder();
        using var DbConnectoin = new NpgsqlConnection(_connectionString);
        DbConnectoin.Open();
        var command = $@"SELECT TABLE_NAME,COLUMN_NAME
                        FROM INFORMATION_SCHEMA.COLUMNS
                        WHERE 1=1
                        AND TABLE_SCHEMA = 'public'
	                    AND TABLE_NAME = '{tableName}'";
        using var pgCommand = new NpgsqlCommand(command, DbConnectoin);
        using var reader = pgCommand.ExecuteReader();
        while (reader.Read())
        {
            builder.Append($"{reader["COLUMN_NAME"].ToString()},");
        }

        reader.Close();
        pgCommand.Dispose();
        DbConnectoin.Close();

        return string.Join(Environment.NewLine,
            builder.ToString()
            .Split(',')
            .Distinct()
            .Where(x => !string.IsNullOrEmpty(x))
            .ToList());
    }
}