namespace El_Nabaash.API.DTOs.Identity;

public record UpdateProfileRequestDto
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
}