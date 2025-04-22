namespace Domain.Models;

public class User
{
    public string Id { get; set; } = null!;
    public string? Image { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; } = null!;
    public string? JobTitle { get; set; }
    public string? Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public UserAddress? Address { get; set; }
    public string DisplayName => FirstName + " " + LastName;
}
