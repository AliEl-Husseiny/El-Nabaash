namespace El_Nabaash.API;

public static class Program
{
    public static async Task Main(string[] args)
    {
        #region Dependency Injection Container

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddWebApiServices(builder.Configuration);
        builder.Services.AddOpenApiSwagger();
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddCoreServices(builder.Configuration);

        #endregion

        #region Middlewares - Pipeline

        var app = builder.Build();

        await app.SeedDatabaseAsync();

        app.UseSwaggerMiddlewares();

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseBlockIdentityEndpoints();

        app.MapEndpoints();

        await app.RunAsync();

        #endregion
    }
}