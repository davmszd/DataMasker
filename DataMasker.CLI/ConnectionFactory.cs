namespace DataMasker.CLI;

public static class ConnectionFactory
{
    static ConnectionFactory()
    {
    }

    public static T GetInstance<T>(string connectionString) where T : IConnectionManager
    {
        return (T)Activator.CreateInstance(typeof(T), connectionString);
    }
}
