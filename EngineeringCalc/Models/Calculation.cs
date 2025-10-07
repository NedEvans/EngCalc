using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngineeringCalc.Models;

public class Calculation
{
    [Key]
    public int CalculationId { get; set; }

    [Required]
    public int JobId { get; set; }

    [Required]
    [MaxLength(200)]
    public string CalculationTitle { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    public int? CurrentRevisionId { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;

    [MaxLength(50)]
    public string Status { get; set; } = "Draft";

    // Navigation properties
    [ForeignKey(nameof(JobId))]
    public Job Job { get; set; } = null!;

    public ICollection<CalculationRevision> Revisions { get; set; } = new List<CalculationRevision>();

    [ForeignKey(nameof(CurrentRevisionId))]
    public CalculationRevision? CurrentRevision { get; set; }
}
