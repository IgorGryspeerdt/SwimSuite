using System.ComponentModel.DataAnnotations;

namespace SwimSuite.Models;

public class TrainingBlock
{
    public Guid Id { get; set; }

    public Guid ClubId { get; set; }

    public Club? Club { get; set; }

    public Guid TrainingGroupId { get; set; }

    public TrainingGroup? TrainingGroup { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    [StringLength(160)]
    public string? Location { get; set; }

    [StringLength(400)]
    public string? Notes { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<TrainerAttendance> TrainerAttendances { get; set; } = new List<TrainerAttendance>();
}
