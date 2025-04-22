using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.ViewModels.Auth;

public class SignInViewModel
{
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
    [Display(Name = "Email", Prompt = "Enter your email address")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Required]
    [RegularExpression(@"^(?=.*\d)(?=.*\W).{8,}$")]
    [Display(Name = "Password", Prompt = "Enter your password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    public bool IsPersisent { get; set; }
}
