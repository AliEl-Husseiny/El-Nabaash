using El_Nabaash.API.Endpoints.Home;
using El_Nabaash.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace El_Nabaash.API;

public class Program
{
    public static void Main(string[] args)
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
        
        var authRouteGroup = app.MapGroup("/api/auth")
            .WithTags("Admin");

        authRouteGroup.MapIdentityApi<ApplicationUser>();
        
        app.MapHomeEndpoints();

        app.Run();
    }
}