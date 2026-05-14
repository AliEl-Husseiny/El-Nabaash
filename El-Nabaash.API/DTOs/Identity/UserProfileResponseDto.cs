using static System.String;

namespace El_Nabaash.API.DTOs.Identity;

public record UserProfileResponseDto
{
    public string? Id { get; init; } = Empty;
    public string? FirstName { get; init; } = Empty;
    public string? LastName { get; init; } = Empty;
    public string? Email { get; init; } = Empty;

}