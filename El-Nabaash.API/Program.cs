namespace El_Nabaash.API;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // get connection string
        var connectionString = DataUtility.GetConnectionString(builder.Configuration);

        // Connect to database
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApiSwagger();


        //add identity endpoints 
        builder.Services.AddIdentityApiEndpoints<ApplicationUser>(opt =>
            {
                opt.SignIn.RequireConfirmedAccount = false;
                opt.User.RequireUniqueEmail = true;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();

        //Admin Policy 
        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));


        //enable validation for minimal APIs
        builder.Services.AddValidation();


        //add email sender services 
        builder.Services.AddTransient<IEmailSender, ConsoleEmailService>();

        // add custom service 
        builder.Services.AddScoped<ISiteService, SiteService>();
        builder.Services.AddScoped<IArtifactMediaFileService, ArtifactMediaFileService>();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseBlockIdentityEndpoints();

        var authRouteGroup = app.MapGroup("/api/auth")
            .WithTags("Admin");

        authRouteGroup.MapIdentityApi<ApplicationUser>();

        
        using (var scope = app.Services.CreateScope())
        {
            await DataSeed.ManageDataAsync(scope.ServiceProvider);
        }
        
        app.MapHomeEndpoints();
        app.MapCustomIdentityEndpoints();
        app.MapSiteEndpoints();
        app.MapArtifactMediaFilesEndpoints();
        await app.RunAsync();
    }
}