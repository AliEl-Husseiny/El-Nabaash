using Npgsql;

namespace El_Nabaash.API.Data;

public class DataUtility
{
    public static string GetConnectionString(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DbConnection");
        var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

        if (databaseUrl == null)
        {
            return connectionString;
        }
        else
        {
            return BuildConnectionString(databaseUrl);
        }

        return connectionString;
    }

    private static string BuildConnectionString(string databaseUrl)
    {
        var databaseUri = new Uri(databaseUrl);
        var userInfo = databaseUri.UserInfo.Split(':');
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = databaseUri.Host,
            Port = databaseUri.Port,
            Username = userInfo[0],
            Password = userInfo[1],
            Database = databaseUri.AbsolutePath.TrimStart('/'),
            SslMode = SslMode.Require,
        };
        return builder.ToString();
    }
}