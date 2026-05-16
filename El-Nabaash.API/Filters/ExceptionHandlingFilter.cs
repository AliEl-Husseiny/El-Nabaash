namespace El_Nabaash.API.Filters;

public class ExceptionHandlingFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        try
        {
            return await next(context);
        }
        catch (Exception ex)
        {
            var env = context.HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
            // Log the exception (you can use any logging framework, e.g., Serilog, NLog, etc.)
            Console.WriteLine($"Exception caught in filter: {ex.Message}");
            // Return a generic error response
            return Results.Problem(
                detail: env.IsDevelopment() ? ex.ToString() : "An unexpected error occurred.",
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal Server Error");
        }
    }
}