using System.ComponentModel.DataAnnotations;

namespace EngineeringCalc.Models;

public class Project
{
    [Key]
    public int ProjectId { get; set; }

    [Required]
    [MaxLength(200)]
    public string ProjectName { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;

    [MaxLength(100)]
    public string? CreatedBy { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = "Active";

    // Navigation properties
    public ICollection<Job> Jobs { get; set; } = new List<Job>();
}
