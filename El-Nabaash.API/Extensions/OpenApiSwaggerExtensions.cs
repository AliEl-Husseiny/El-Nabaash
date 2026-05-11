using Microsoft.OpenApi;

namespace El_Nabaash.API.Extensions;

public static class OpenApiSwaggerExtensions
{
        public static void AddOpenApiSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1" , new OpenApiInfo
                {
                    Title = "El-Nabaash API",
                    Version = "1.0.0",
                    Description = "An API for El-Nabaash application.",
                    Contact = new OpenApiContact
                    {
                        Name = "El-Nabaash Team - Ali Ahmed El-husseiny",
                        Email = "ali.ahmed.software.engineer@gmail.com"
                    }
                });
            });
        }
}