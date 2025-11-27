using Microsoft.AspNetCore.Identity;

namespace COMP2139___assignment2.Models;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? ProfilePictureUrl { get; set; }
}