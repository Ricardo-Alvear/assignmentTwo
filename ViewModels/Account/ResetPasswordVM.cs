using System.ComponentModel.DataAnnotations;

namespace comp2147.ViewModels.Account;

public class ResetPasswordVM
{
    public string Token { get; set; } = string.Empty;

    [Required, EmailAddress] 
    public string Email { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [DataType(DataType.Password), Compare("Password")]
    public string ConfirmPassword { get; set; } = string.Empty;

}