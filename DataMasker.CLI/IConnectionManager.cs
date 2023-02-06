namespace DataMasker.CLI;

public interface IConnectionManager
{
    Task<string> GetTableNamesAsync();

    Task<string> GetTableColumnNamesAsync(string tableName);
}
