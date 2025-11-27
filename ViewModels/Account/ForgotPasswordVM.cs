using System.ComponentModel.DataAnnotations;

namespace COMP2139___assignment2.ViewModels.Account;

public class ForgotPasswordVm
{
    [Required, EmailAddress] public string Email { get; set; } = "";
}