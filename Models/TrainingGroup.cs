using System.ComponentModel.DataAnnotations;

namespace SwimSuite.Models;

public class TrainingGroup
{
    public Guid Id { get; set; }

    public Guid ClubId { get; set; }

    public Club? Club { get; set; }

    [Required]
    [StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [StringLength(400)]
    public string? Description { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<TrainingBlock> TrainingBlocks { get; set; } = new List<TrainingBlock>();
}
