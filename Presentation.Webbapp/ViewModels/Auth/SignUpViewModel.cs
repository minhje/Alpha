using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.ViewModels.Auth;

public class SignUpViewModel
{
    [Required(ErrorMessage = "Required")]
    [Display(Name = "First Name", Prompt = "Enter first name")]
    [DataType(DataType.Text)]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Last Name", Prompt = "Enter last name")]
    [DataType(DataType.Text)]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
    [Display(Name = "Email", Prompt = "Enter email address")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Display(Name = "Phone", Prompt = "Enter phone number")]
    [DataType(DataType.PhoneNumber)]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Required")]
    [RegularExpression(@"^(?=.*\d)(?=.*\W).{8,}$")]
    [Display(Name = "Password", Prompt = "Enter password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    [Display(Name = "Confirm Password", Prompt = "Confirm password")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = null!;

    [Range(typeof(bool), "true", "true")]
    public bool TermsAndConditions { get; set; } 
}
