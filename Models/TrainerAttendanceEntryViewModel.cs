using System.ComponentModel.DataAnnotations;

namespace SwimSuite.Models;

public class TrainerAttendanceEntryViewModel
{
    public Guid TrainerId { get; set; }

    public string TrainerName { get; set; } = string.Empty;

    [Display(Name = "Present")]
    public bool IsPresent { get; set; }

    [StringLength(400)]
    public string? Notes { get; set; }
}
