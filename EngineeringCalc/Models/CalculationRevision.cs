using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngineeringCalc.Models;

public class CalculationRevision
{
    [Key]
    public int RevisionId { get; set; }

    [Required]
    public int CalculationId { get; set; }

    [Required]
    [MaxLength(50)]
    public string RevisionNumber { get; set; } = "Rev1";

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [MaxLength(100)]
    public string? CreatedBy { get; set; }

    [MaxLength(1000)]
    public string? Comments { get; set; }

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Draft"; // Draft/Approved/Superseded

    // Navigation properties
    [ForeignKey(nameof(CalculationId))]
    public Calculation Calculation { get; set; } = null!;

    public ICollection<CardInstance> CardInstances { get; set; } = new List<CardInstance>();
}
