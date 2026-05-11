namespace El_Nabaash.API.Extensions;

public static class OpenApiSwaggerExtensions
{
        public static void AddOpenApiSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }
}