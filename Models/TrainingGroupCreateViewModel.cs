using System.ComponentModel.DataAnnotations;

namespace SwimSuite.Models;

public class TrainingGroupCreateViewModel
{
    public Guid ClubId { get; set; }

    [Required]
    [StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [StringLength(400)]
    public string? Description { get; set; }
}
