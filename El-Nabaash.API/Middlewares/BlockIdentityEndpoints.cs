
namespace El_Nabaash.API.Middlewares;

// using the primary instructor
public class BlockIdentityEndpoints(RequestDelegate next)
{
    private static readonly string[] BlockedPaths =
    [
        "api/auth/forgotpassword",
        "api/auth/register",
        "api/auth/resetpassword",
        "api/auth/manage",
        "api/auth/info"
    ];

    public async Task InvokeAsync(HttpContext context)
    {
        // Before logic

        var requestPath = context.Request.Path.Value?.ToLowerInvariant();
        if (requestPath is not null && BlockedPaths.Contains(requestPath))
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync("Not Found");
            return; // Short-circuit the pipeline
        }
        
        await next(context);

        // After logic
    }
}

public static class BlockIdentityEndpointsExtensions
{
    public static IApplicationBuilder UseBlockIdentityEndpoints(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<BlockIdentityEndpoints>();
    }
}