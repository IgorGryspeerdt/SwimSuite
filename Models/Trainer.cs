using System.ComponentModel.DataAnnotations;

namespace SwimSuite.Models;

public class Trainer
{
    public Guid Id { get; set; }

    public Guid ClubId { get; set; }

    public Club? Club { get; set; }

    [Required]
    [StringLength(120)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(120)]
    public string LastName { get; set; } = string.Empty;

    [EmailAddress]
    [StringLength(160)]
    public string? Email { get; set; }

    [Phone]
    [StringLength(80)]
    public string? PhoneNumber { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public string DisplayName => $"{FirstName} {LastName}".Trim();

    public ICollection<TrainerAttendance> Attendances { get; set; } = new List<TrainerAttendance>();
}
