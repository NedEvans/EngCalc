using System.ComponentModel.DataAnnotations;

namespace EngineeringCalc.Models;

public class Card
{
    [Key]
    public int CardId { get; set; }

    [Required]
    [MaxLength(200)]
    public string CardName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string CardType { get; set; } = string.Empty; // e.g., "Tension Check", "Moment Check"

    [MaxLength(50)]
    public string CardVersion { get; set; } = "1.0";

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    public string CodeTemplate { get; set; } = string.Empty; // C# code

    public string? MathMLFormula { get; set; }

    [Required]
    public string InputVariables { get; set; } = "{}"; // JSON schema

    [Required]
    public string OutputVariables { get; set; } = "{}"; // JSON schema

    [MaxLength(100)]
    public string? DesignLoadVariable { get; set; }

    [MaxLength(100)]
    public string? CapacityVariable { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<CardInstance> CardInstances { get; set; } = new List<CardInstance>();
}
