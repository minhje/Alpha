using Domain.Models;

namespace Domain.Dtos;

public class EditProjectFormData
{
    public string? ProjectImage { get; set; }
    public string ProjectName { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? Budget { get; set; }
    public Client Client { get; set; } = null!;
    public List<string> SelectedClientIds { get; set; } = [];
    public string? MemberId { get; set; }
    public int StatusId { get; set; }

}