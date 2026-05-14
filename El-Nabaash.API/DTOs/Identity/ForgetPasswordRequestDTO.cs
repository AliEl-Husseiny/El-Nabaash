namespace El_Nabaash.API.DTOs.Identity;

public record ForgetPasswordRequestDto
{
    public string Email { get; init; } = string.Empty;
}