using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.WebUtilities;
using static System.String;

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
        group.MapPost("reset-password", ResetPassword)
            .WithName("ResetPassword")
            .WithDescription("Custom Reset Password for users")
            .WithSummary("Custom Reset Password");


        group.MapPost("/forget-password", ForgetPassword)
            .WithName("ForgetPassword")
            .WithDescription("Custom Forget Password")
            .WithSummary("Custom forget password")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);


        group.MapGet("manage/profile", GetProfileInfo)
            .WithName("GetProfileInfo")
            .WithDescription("Get Current user profile info")
            .WithSummary("Get the current users profile")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequireAuthorization();


        group.MapPut("/manage/profile", UpdateProfile)
            .WithName("UpdateProfile")
            .WithDescription("Updates the current user profile information")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);

        // step 3 - implement route handlers

        group.MapGet("/manage/users", ListAllUsers)
            .WithName("ListUsers")
            .WithDescription("List all users in the system")
            .WithSummary("List all users")
            .RequireAuthorization()
            .Produces<IEnumerable<UserProfileResponseDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        // step 4 - return the route 
        return routeBuilder;
    }

    private static async Task<IResult> UpdateProfile(
        ClaimsPrincipal principal,
        UserManager<ApplicationUser> userManager,
        UpdateProfileRequestDto? updateProfileRequestDto
    )
    {
        // validate inputs
        if (updateProfileRequestDto is null || IsNullOrEmpty(updateProfileRequestDto.FirstName) ||
            IsNullOrEmpty(updateProfileRequestDto.LastName))
        {
            return Results.BadRequest("First name and last name are required");
        }

        var user = await userManager.GetUserAsync(principal);
        if (user is null) return Results.BadRequest("User not found");

        user.FirstName = updateProfileRequestDto.FirstName;
        user.LastName = updateProfileRequestDto.LastName;

        var result = await userManager.UpdateAsync(user);
        return result.Succeeded
            ? Results.Ok(new { Message = "Profile updated successfully" })
            : Results.BadRequest(new { Message = "Failed to update profile", result.Errors });
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
        if (await userManager.FindByEmailAsync(registerUserRequestDto.Email) is not null)
            return Results.BadRequest($"User with this email {registerUserRequestDto} already exist");

        var user = new ApplicationUser
        {
            UserName = registerUserRequestDto.Email,
            Email = registerUserRequestDto.Email,
            FirstName = registerUserRequestDto.FirstName,
            LastName = registerUserRequestDto.LastName
        };

        // In real Co-Operations we generate the passwords 
        const string tempPassword = "TempPassword123!";

        var created = await userManager.CreateAsync(user, tempPassword);

        if (!created.Succeeded) return Results.BadRequest(new { created.Errors });


        if (!await roleManager.RoleExistsAsync("Researcher"))
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole("Researcher"));
            if (!roleResult.Succeeded)
            {
                return Results.BadRequest(new { roleResult.Errors });
            }
        }

        var addToRoleResult = await userManager.AddToRoleAsync(user, "Researcher");
        if (!addToRoleResult.Succeeded)
        {
            return Results.BadRequest(new { addToRoleResult.Errors });
        }

        // Create a token 

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));


        //send email to user to change password
        var baseUrl = configuration["BaseURL"] ?? "https://localhost:7078";

        await emailSender.SendEmailAsync(
            registerUserRequestDto.Email,
            "Welcome to El-Nabaash",
            $"""
                             <h1>Welcome to El-Nabaash</h1>
                             <p>Your account has been created successfully. please change your password by visiting: ${baseUrl}/setpassword.html
                             {baseUrl}/Setpassword.html?email{registerUserRequestDto.Email}&resetCode={encodedToken}
                             <p>Your temporary password is: {tempPassword}</p>
             """
        );


        return Results.Ok(new
        {
            Message =
                $"User {registerUserRequestDto.Email} Created Successfully, Please check your email to set your password"
        });
    }

    private static async Task<IResult> ResetPassword(
        ResetPasswordRequest resetPasswordRequest,
        UserManager<ApplicationUser> userManager)
    {
        if (IsNullOrEmpty(resetPasswordRequest.Email) ||
            IsNullOrEmpty(resetPasswordRequest.NewPassword) ||
            IsNullOrEmpty(resetPasswordRequest.ResetCode))
        {
            return Results.BadRequest("All fields are required.");
        }

        // find the user 
        var user = await userManager.FindByEmailAsync(resetPasswordRequest.Email);
        if (user is null) return Results.BadRequest($"User with Email: {resetPasswordRequest.Email} not found");


        try
        {
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPasswordRequest.ResetCode));
            var resetResult =
                await userManager.ResetPasswordAsync(user, decodedToken, resetPasswordRequest.NewPassword);
            if (resetResult.Succeeded) return Results.Ok(new { Message = "Password reset successful." });
            return Results.BadRequest(new { Message = "Password reset failed.", resetResult.Errors });
        }
        catch (FormatException)
        {
            return Results.BadRequest("Invalid reset code format.");
        }
        catch (Exception e)
        {
            return Results.BadRequest($"Invalid reset Password Error: {e.Message}");
        }
    }


    private static async Task<IResult> ForgetPassword(
        ForgetPasswordRequestDto forgetPasswordRequestDto,
        UserManager<ApplicationUser> userManager,
        IEmailSender emailSender,
        IConfiguration configuration
    )
    {
        // check if the email is null or empty 
        if (IsNullOrEmpty(forgetPasswordRequestDto.Email))
            return Results.BadRequest("Email is required");

        // check if the user with the email is existed 
        var user = await userManager.FindByEmailAsync(forgetPasswordRequestDto.Email);
        if (user is null) return Results.BadRequest($"The Reset Password link has been sent to this email if it exist");


        // generate a reset password token 
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        // encode the token 
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        // get the baseUrl from the configurations 
        var baseUrl = configuration["BaseURL"] ?? "https://localhost:7078";

        // build the reset link 
        var resetLink = $"{baseUrl}/resetpassword.html?email={forgetPasswordRequestDto.Email}&resetCode={encodedToken}";

        // send the email 
        await emailSender.SendEmailAsync(
            forgetPasswordRequestDto.Email,
            "Password Reset Request",
            $"""
                             <h1>Password Reset Request</h1>
                             <p>We received a request to reset your password. Please click the link below to reset your password:</p>
                             <a href="{resetLink}">Reset Password</a>
                             <p>If you did not request a password reset, please ignore this email.</p>
             """
        );


        return Results.Ok(new { Message = "Forget Password Endpoint" });
    }


    // Get profile Info 
    private static async Task<IResult> GetProfileInfo(
        ClaimsPrincipal principal,
        UserManager<ApplicationUser> userManager
    )
    {
        var user = await userManager.GetUserAsync(principal);
        if (user is null) return Results.BadRequest("User not found");

        return Results.Ok(new UserProfileResponseDto()
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
        });
    }
    
    private static async Task<IResult> ListAllUsers(UserManager<ApplicationUser> userManager)
    {
        var users = await userManager.Users.ToListAsync();
        var userDtos = users.Select(user => new UserProfileResponseDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName
        }).ToList();

        return Results.Ok(userDtos);
    }
}