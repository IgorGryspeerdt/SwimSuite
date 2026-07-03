using System.ComponentModel.DataAnnotations;

namespace SwimSuite.Models;

public class TrainerCreateViewModel
{
    public Guid ClubId { get; set; }

    [Required]
    [StringLength(120)]
    [Display(Name = "First name")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(120)]
    [Display(Name = "Last name")]
    public string LastName { get; set; } = string.Empty;

    [EmailAddress]
    [StringLength(160)]
    public string? Email { get; set; }

    [Phone]
    [StringLength(80)]
    [Display(Name = "Phone number")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;
}
