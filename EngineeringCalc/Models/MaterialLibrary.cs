using System.ComponentModel.DataAnnotations;

namespace EngineeringCalc.Models;

public class MaterialLibrary
{
    [Key]
    public int MaterialId { get; set; }

    [Required]
    [MaxLength(100)]
    public string MaterialType { get; set; } = string.Empty; // Steel/Concrete/Timber

    [Required]
    [MaxLength(100)]
    public string Grade { get; set; } = string.Empty; // e.g., "Grade 300", "N40 Concrete"

    [Required]
    public string Properties { get; set; } = "{}"; // JSON - yield strength, ultimate strength, etc.

    [MaxLength(100)]
    public string? Standard { get; set; } // e.g., "AS4100", "AS3600"

    [MaxLength(500)]
    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}
