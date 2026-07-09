namespace El_Nabaash.API.Extensions;

public static class WebApiServiceExtensions
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // add authorization
        services.AddAuthorization();

        // Admin Policy
        services.AddAuthorizationBuilder()
            .AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));

        // enable validation for minimal APIs
        services.AddValidation();

        return services;
    }
}
