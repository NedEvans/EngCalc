using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngineeringCalc.Models;

public class GlobalConstant
{
    [Key]
    public int GlobalConstantId { get; set; }

    [Required]
    public int JobId { get; set; }

    // Optional reference to AppConstant (null if custom job-level constant)
    public int? AppConstantId { get; set; }

    [Required]
    [MaxLength(100)]
    public string ConstantName { get; set; } = string.Empty; // e.g., "SteelYieldStrength"

    [Required]
    public string ConstantValue { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Unit { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey(nameof(JobId))]
    public Job Job { get; set; } = null!;

    [ForeignKey(nameof(AppConstantId))]
    public AppConstant? AppConstant { get; set; }
}
