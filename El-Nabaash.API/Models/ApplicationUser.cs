using System.ComponentModel.DataAnnotations;

namespace El_Nabaash.API.Models;

public class ApplicationUser : IdentityUser
{
    // Extend it with the additional properties you want to store for your users
    [Required] public string? FirstName { get; set; }
    [Required] public string? LastName { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}