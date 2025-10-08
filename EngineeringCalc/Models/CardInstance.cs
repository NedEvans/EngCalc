using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngineeringCalc.Models;

public class CardInstance
{
    [Key]
    public int CardInstanceId { get; set; }

    [Required]
    public int CardId { get; set; }

    [Required]
    public int CalculationRevisionId { get; set; }

    public int DisplayOrder { get; set; } = 0;

    [Required]
    public string LocalVariables { get; set; } = "{}"; // JSON - actual values for local overrides

    // Maps variable name to GlobalConstantId for bound variables
    public string? GlobalConstantBindings { get; set; } // JSON: { "fy": 123, "Es": 124 }

    // Tracks which bound variables are overridden locally (true = use local, false = use global)
    public string? GlobalConstantOverrides { get; set; } // JSON: { "fy": false, "Es": true }

    // Snapshot of actual input values used in last calculation (for audit trail)
    public string? InputSnapshot { get; set; } // JSON: { "fy": 350, "Es": 200000, "thickness": 10 }

    public string? CalculatedResults { get; set; } // JSON

    public double? DesignLoad { get; set; }

    public double? CalculatedCapacity { get; set; }

    [MaxLength(50)]
    public string? CheckResult { get; set; } // Pass/Fail/Warning

    public DateTime? LastCalculated { get; set; }

    // Navigation properties
    [ForeignKey(nameof(CardId))]
    public Card Card { get; set; } = null!;

    [ForeignKey(nameof(CalculationRevisionId))]
    public CalculationRevision CalculationRevision { get; set; } = null!;
}
