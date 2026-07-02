using System.ComponentModel.DataAnnotations;

namespace SwimSuite.Models;

public class Club
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(160)]
    public string Name { get; set; } = string.Empty;

    [StringLength(40)]
    public string? RegistrationNumber { get; set; }

    [StringLength(160)]
    public string? Email { get; set; }

    [StringLength(80)]
    public string? PhoneNumber { get; set; }

    [StringLength(240)]
    public string? Address { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public ICollection<TrainingGroup> TrainingGroups { get; set; } = new List<TrainingGroup>();

    public ICollection<TrainingBlock> TrainingBlocks { get; set; } = new List<TrainingBlock>();
}
