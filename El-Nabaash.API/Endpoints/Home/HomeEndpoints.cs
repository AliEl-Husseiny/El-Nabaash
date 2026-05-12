using Microsoft.AspNetCore.Http.HttpResults;

namespace El_Nabaash.API.Endpoints.Home;

public static class HomeEndpoints
{
    public static IEndpointRouteBuilder MapHomeEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var homeGroup = routeBuilder.MapGroup("api/Home")
            .WithTags("Home");

        // api/Home/welcome
        homeGroup.MapGet("/welcome", GetWelcomeMessage)
            .WithName("GetWelcomeMessage")
            .WithSummary("Welcome Message")
            .WithDescription("Displays a welcome message");
        return routeBuilder;
    }

    // Handlers 

    private static async Task<Ok<WelcomeResponse>> GetWelcomeMessage(CancellationToken ct)
    {
        var welcomeMessage = new WelcomeResponse
        {
            Message = "Welcome to El-Nabaash API!",
            Version = "1.0.0",
            TimeOnly = DateTime.Now.ToShortTimeString()
        };
        return TypedResults.Ok(welcomeMessage);
    }
}