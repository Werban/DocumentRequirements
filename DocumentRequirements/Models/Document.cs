using System.ComponentModel.DataAnnotations;

public class Document
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(500)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Designation { get; set; } = string.Empty;

    [Required]
    public DateTime EffectiveDate { get; set; }

    public ICollection<Requirement> Requirements { get; set; } = new List<Requirement>();
}