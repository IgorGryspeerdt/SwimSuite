using System.ComponentModel.DataAnnotations;

namespace SwimSuite.Models;

public class ClubCreateViewModel
{
    [Required]
    [StringLength(160)]
    public string Name { get; set; } = string.Empty;

    [StringLength(40)]
    [Display(Name = "Registration number")]
    public string? RegistrationNumber { get; set; }

    [EmailAddress]
    [StringLength(160)]
    public string? Email { get; set; }

    [Phone]
    [StringLength(80)]
    [Display(Name = "Phone number")]
    public string? PhoneNumber { get; set; }

    [StringLength(240)]
    public string? Address { get; set; }
}
