using System.ComponentModel.DataAnnotations;

namespace SwimSuite.Models;

public class TrainingBlockCreateViewModel
{
    public Guid ClubId { get; set; }

    [Display(Name = "Training group")]
    public Guid TrainingGroupId { get; set; }

    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Display(Name = "Start time")]
    public TimeOnly StartTime { get; set; } = new(18, 0);

    [Display(Name = "End time")]
    public TimeOnly EndTime { get; set; } = new(19, 0);

    [StringLength(160)]
    public string? Location { get; set; }

    [StringLength(400)]
    public string? Notes { get; set; }
}
