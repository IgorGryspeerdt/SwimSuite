using System.ComponentModel.DataAnnotations;

namespace SwimSuite.Models;

public class OfficialDutyCreateViewModel
{
    public Guid ClubId { get; set; }

    [Required]
    [Display(Name = "Official")]
    public Guid OfficialId { get; set; }

    [Required]
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Required]
    [StringLength(160)]
    [Display(Name = "Meet name")]
    public string MeetName { get; set; } = string.Empty;

    [Required]
    [StringLength(120)]
    public string Role { get; set; } = string.Empty;

    [StringLength(160)]
    public string? Location { get; set; }

    [StringLength(400)]
    public string? Notes { get; set; }
}
