using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngineeringCalc.Models;

public class Job
{
    [Key]
    public int JobId { get; set; }

    [Required]
    public int ProjectId { get; set; }

    [Required]
    [MaxLength(200)]
    public string JobName { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;

    [MaxLength(50)]
    public string Status { get; set; } = "Active";

    // Navigation properties
    [ForeignKey(nameof(ProjectId))]
    public Project Project { get; set; } = null!;

    public ICollection<Calculation> Calculations { get; set; } = new List<Calculation>();
    public ICollection<GlobalVariable> GlobalVariables { get; set; } = new List<GlobalVariable>();
}
