using System.ComponentModel.DataAnnotations;

namespace AiJobAnalyzer.ViewModels;

public class JobAnalyzeViewModel
{
    [Required]
    public string JobText { get; set; } = "";

    public string? Result { get; set; }

    public bool IsDemoMode { get; set; }
}
