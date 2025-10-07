using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngineeringCalc.Models;

public class GlobalVariable
{
    [Key]
    public int GlobalVariableId { get; set; }

    [Required]
    public int JobId { get; set; }

    [Required]
    [MaxLength(100)]
    public string VariableName { get; set; } = string.Empty; // e.g., "SteelYieldStrength"

    [Required]
    public string VariableValue { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Unit { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    // Navigation properties
    [ForeignKey(nameof(JobId))]
    public Job Job { get; set; } = null!;
}
