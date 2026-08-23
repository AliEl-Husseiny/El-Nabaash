namespace El_Nabaash.API.Extensions;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // get connection string
        var connectionString = DataUtility.GetConnectionString(configuration);

        // Connect to database
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));

        // add identity endpoints
        services.AddIdentityApiEndpoints<ApplicationUser>(opt =>
            {
                opt.SignIn.RequireConfirmedAccount = false;
                opt.User.RequireUniqueEmail = true;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();

        // add email sender services
        services.AddTransient<IEmailSender, ConsoleEmailService>();

        return services;
    }
}
