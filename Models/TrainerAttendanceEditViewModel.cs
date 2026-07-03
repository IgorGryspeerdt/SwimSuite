namespace SwimSuite.Models;

public class TrainerAttendanceEditViewModel
{
    public Guid ClubId { get; set; }

    public Guid TrainingBlockId { get; set; }

    public string ClubName { get; set; } = string.Empty;

    public string TrainingGroupName { get; set; } = string.Empty;

    public DateOnly Date { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public List<TrainerAttendanceEntryViewModel> Trainers { get; set; } = [];
}
