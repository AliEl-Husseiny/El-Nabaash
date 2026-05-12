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
                    Description = """
                                  <img src = "/images/AeonRegistryLogo.png" height="120" />
                                  
                                  ## Aeon Research Division
                                  
                                  Internal API for managing recovered artifacts and research data. 
                                  Provides Secure access for field researchers and analysts.
                                  
                                  ### Key Features:
                                  - Site and Artifact Catalog
                                  - Research record submissions
                                  - Secure media storage
                                  - User role management
                                  """,
                    Contact = new OpenApiContact
                    {
                        Name = "El-Nabaash Team - Ali Ahmed El-husseiny",
                        Email = "ali.ahmed.software.engineer@gmail.com",
                        Url = new Uri("https://linktr.ee/ali.ahmed.software.engineer")
                    }
                });
                opt.AddSecurityDefinition("Bearer" , new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter Bearer [space] and then your valid JWT token."
                });
                
                opt.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecuritySchemeReference("Bearer",document),
                        new List<string>()
                    }
                });


                string[] hiddenEndpoints =
                [
                    "api/auth/register",
                    "api/auth/refresh",
                    "api/auth/confirmemail",
                    "api/auth/resendconfirmationemail",
                    "api/auth/forgotpassword",
                    "api/auth/resetpassword",
                    "api/auth/manage",
                    "api/auth/manage/info",
                    "api/auth/manage/2fa"
                ];
                
                opt.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var path = apiDesc.RelativePath?.ToLower();
                    return !(hiddenEndpoints.Contains(path,StringComparer.OrdinalIgnoreCase) | path is null);
                    // Exclude these endpoints from the documentation
                    // Include all other endpoints
                });
            });
        }
}