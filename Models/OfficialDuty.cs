using System.ComponentModel.DataAnnotations;

namespace SwimSuite.Models;

public class OfficialDuty
{
    public Guid Id { get; set; }

    public Guid ClubId { get; set; }

    public Club? Club { get; set; }

    public Guid OfficialId { get; set; }

    public Official? Official { get; set; }

    public DateOnly Date { get; set; }

    [Required]
    [StringLength(160)]
    public string MeetName { get; set; } = string.Empty;

    [Required]
    [StringLength(120)]
    public string Role { get; set; } = string.Empty;

    [StringLength(160)]
    public string? Location { get; set; }

    [StringLength(400)]
    public string? Notes { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
