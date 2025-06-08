using Microsoft.Data.SqlClient;

public static class DBContext
{
    private static string _connectionString;

    public static void Initialize(string connectionString)
    {
        _connectionString = connectionString;
    }

    public static SqlConnection GetConnection()
    {
        if (string.IsNullOrEmpty(_connectionString))
            throw new InvalidOperationException("DBContext is not initialized. Call Initialize() first.");

        return new SqlConnection(_connectionString);
    }
}

