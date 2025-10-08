using System.ComponentModel.DataAnnotations;

namespace EngineeringCalc.Models;

public class AppConstant
{
    [Key]
    public int AppConstantId { get; set; }

    [Required]
    [MaxLength(100)]
    public string ConstantName { get; set; } = string.Empty;

    [Required]
    public string DefaultValue { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Unit { get; set; }

    [MaxLength(100)]
    public string? Standard { get; set; } // e.g., "AS4100", "AS3600"

    [MaxLength(100)]
    public string? Category { get; set; } // e.g., "Steel", "Concrete", "Factors"

    [MaxLength(500)]
    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<GlobalConstant> GlobalConstants { get; set; } = new List<GlobalConstant>();
}
