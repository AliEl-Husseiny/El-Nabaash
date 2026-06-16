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

    
}