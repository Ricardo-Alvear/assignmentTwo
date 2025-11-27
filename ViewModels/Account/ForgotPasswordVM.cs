using System.ComponentModel.DataAnnotations;

namespace comp2147.ViewModels.Account;

public class ForgotPasswordVm
{
    [Required, EmailAddress] public string Email { get; set; } = "";
}