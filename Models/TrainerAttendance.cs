using System.ComponentModel.DataAnnotations;

namespace SwimSuite.Models;

public class TrainerAttendance
{
    public Guid Id { get; set; }

    public Guid ClubId { get; set; }

    public Club? Club { get; set; }

    public Guid TrainingBlockId { get; set; }

    public TrainingBlock? TrainingBlock { get; set; }

    public Guid TrainerId { get; set; }

    public Trainer? Trainer { get; set; }

    public bool IsPresent { get; set; }

    [StringLength(400)]
    public string? Notes { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
