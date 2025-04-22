namespace Domain.Dtos;

public class AddClientFormData
{
    public string ClientName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Location { get; set; }
    public string? Phone { get; set; }
}
