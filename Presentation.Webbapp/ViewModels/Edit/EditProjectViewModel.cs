using Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.ViewModels.Edit;

public class EditProjectViewModel
{
    public string Id { get; set; } = null!;

    [Display(Name = "Project Image", Prompt = "Select a image")]
    [DataType(DataType.Upload)]
    public IFormFile? ProjectImage { get; set; }
    public string? ExistingImagePath { get; set; }

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Project Name", Prompt = "Enter project name")]
    [DataType(DataType.Text)]
    public string ProjectName { get; set; } = null!;

    [Display(Name = "Description", Prompt = "Enter project description")]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }

    [Display(Name = "Start Date", Prompt = "Enter project start date")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; } = DateTime.Now;

    [Display(Name = "End Date", Prompt = "Enter project end date")]
    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }

    [Display(Name = "Budget", Prompt = "Enter project budget")]
    [DataType(DataType.Currency)]
    public decimal? Budget { get; set; }

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Client Name", Prompt = "Select client")]
    public string? Client { get; set; }
    public string? SelectedClientIds { get; set; }

    [Display(Name = "Project Manager", Prompt = "Select project manager")]
    public User? Member { get; set; }

    [Display(Name = "Status", Prompt = "Select project status")]
    public Status? Status { get; set; }
}