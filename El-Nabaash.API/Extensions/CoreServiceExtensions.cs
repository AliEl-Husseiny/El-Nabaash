namespace El_Nabaash.API.Extensions;

public static class CoreServiceExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // register custom services
        services.AddScoped<ISiteService, SiteService>();
        services.AddScoped<IArtifactMediaFileService, ArtifactMediaFileService>();
        services.AddScoped<IArtifactService, ArtifactService>();
        services.AddScoped<ICatalogRecordService, CatalogRecordService>();

        return services;
    }
}
