namespace El_Nabaash.API.Data;

public class DataUtility
{
    public static string GetConnectionString(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DbConnection") ?? "";
        return connectionString;
    }
}