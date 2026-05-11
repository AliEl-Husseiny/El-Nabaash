namespace El_Nabaash.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();
        
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApiSwagger();
        

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseStaticFiles();

        app.MapGet("/welcome", () =>
        {
            var response = new
            {
                Message = "Welcome to El-Nabaash API!",
                Version = "1.0.0",
                TimeOnly = DateTime.Now.ToShortTimeString()
            };
            return response;
        }).WithName("Welcome");

        app.Run();
    }
}