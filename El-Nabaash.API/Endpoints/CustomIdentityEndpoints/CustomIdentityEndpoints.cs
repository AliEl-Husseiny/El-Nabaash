
using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace El_Nabaash.API.Endpoints.CustomIdentityEndpoints;

public static class CustomIdentityEndpoints
{
    public static IEndpointRouteBuilder MapCustomIdentityEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        // step 1 - make a group 
        var group = routeBuilder.MapGroup("api/auth")
            .WithTags("Admin");
        
        /*
         * we can not implement endpoint with Register name
         * as the Identity already has a Register endpoint
         * so we have two ways to solve this problem
         * 1- we can change the name of the endpoint to something else like RegisterAdmin or RegisterUser
         * 2- we can use the same name but with different route like api/auth/register and api/auth/admin/register
         * I will go with the first option to avoid confusion and to keep the route clean and simple
         */
        
        // step 2 - make endpoints in the group
        group.MapPost("/register-admin", RegisterUser)
            .WithName("RegisterAdmin")
            .WithSummary("Register Admin User")
            .WithDescription("Registers a new admin user with admin roles");
        // .RequireAuthorization("AdminPolicy"); disabled for testing


        // step 3 - implement route handlers
        



        // step 4 - return the route 
        return routeBuilder;
    }
    
    
    
    
    // Handlers
    private static async Task<IResult> RegisterUser(
        RegisterUserRequestDto registerUserRequestDto,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IEmailSender emailSender,
        IConfiguration configuration
        )
    
    
    {
        // check if the user email is already exists
        if(await userManager.FindByEmailAsync(registerUserRequestDto.Email) is not null)
        {
            return Results.BadRequest($"User with this email {registerUserRequestDto} already exist");
        }

        var user = new ApplicationUser()
        {
            UserName = registerUserRequestDto.Email,
            Email = registerUserRequestDto.Email,
            FirstName = registerUserRequestDto.FirstName,
            LastName = registerUserRequestDto.LastName,
        };

        // In real Co-Operations we generate the passwords 
        var tempPassword = "TempPassword123!";
        
        var created = await userManager.CreateAsync(user, tempPassword);

        if (!created.Succeeded)
        {
            return Results.BadRequest(new { Errors = created.Errors });
        }


        if (!await roleManager.RoleExistsAsync("Researcher"))
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole("Researcher"));
            if (!roleResult.Succeeded)
            {
                return Results.BadRequest(new { Errors = roleResult.Errors });
            }
        }
        
        var addedToRoleResult = await userManager.AddToRoleAsync(user, "Researcher");
        if(!addedToRoleResult.Succeeded)
        {
            return Results.BadRequest(new { Errors = addedToRoleResult.Errors });
        }
        
        
        // Create a token 
        
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        
        
        
        //send email to user to change password
        var baseURL = configuration["BaseURL"] ?? "https://localhost:7078";

        await emailSender.SendEmailAsync(
            registerUserRequestDto.Email,
            "Welcome to El-Nabaash",
            $"""
                <h1>Welcome to El-Nabaash</h1>
                <p>Your account has been created successfully. please change your password by visiting: ${baseURL}/Setpassword.html
                {baseURL}/Setpassword.html?email{registerUserRequestDto.Email}&resetCode={encodedToken}
                <p>Your temporary password is: {tempPassword}</p>
             """
        );

        
        return Results.Ok(new {Message = $"User {registerUserRequestDto.Email} Created Successfully, Please check your email to set your password"});

    }
}