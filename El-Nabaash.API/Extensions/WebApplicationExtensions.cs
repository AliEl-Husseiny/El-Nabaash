namespace El_Nabaash.API.Extensions;

public static class WebApplicationExtensions
{
    public static async Task SeedDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        await DataSeed.ManageDataAsync(scope.ServiceProvider);
    }

    public static WebApplication UseSwaggerMiddlewares(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        return app;
    }

    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var authRouteGroup = app.MapGroup("/api/auth")
            .WithTags("Admin");

        authRouteGroup.MapIdentityApi<ApplicationUser>();

        app.MapHomeEndpoints();
        app.MapCustomIdentityEndpoints();
        app.MapSiteEndpoints();
        app.MapArtifactMediaFilesEndpoints();
        app.MapArtifactEndpoints();
        app.MapCatalogRecordEndpoints();

        return app;
    }
}
