using Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.ViewModels.Add;

public class AddMemberViewModel
{
    [Display(Name = "Member Image", Prompt = "Select a image")]
    [DataType(DataType.Upload)]
    public IFormFile? UserImage { get; set; }

    [Display(Name = "First Name", Prompt = "Enter first name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Last Name", Prompt = "Enter last name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string LastName { get; set; } = null!;

    [Display(Name = "Email", Prompt = "Enter email address")]
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Required")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "You must enter a valid email address.")]
    public string Email { get; set; } = null!;

    [Display(Name = "Streetname", Prompt = "Enter Streetname")]
    [DataType(DataType.Text)]
    public UserAddress StreetName { get; set; } = null!;

    [Display(Name = "City", Prompt = "Enter City")]
    [DataType(DataType.Text)]
    public string City { get; set; } = null!;

    [Display(Name = "Postal Code", Prompt = "Enter postal code")]
    [DataType(DataType.Text)]
    public string PostalCode { get; set; } = null!;

    [Display(Name = "Phone", Prompt = "Enter phone number")]
    [DataType(DataType.PhoneNumber)]
    public string? Phone { get; set; }

    [Display(Name = "Job Title", Prompt = "Your job title")]
    [DataType(DataType.Text)]
    public string JobTitle { get; set; } = null!;
}
