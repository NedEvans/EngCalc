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
    public string LocalVariables { get; set; } = "{}"; // JSON - actual values

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
