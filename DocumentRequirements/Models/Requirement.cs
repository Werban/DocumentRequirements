using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
public class Requirement
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Поле 'DocumentId' обязательно для заполнения.")]
    public Guid DocumentId { get; set; }

    [Required(ErrorMessage = "Поле 'RequirementDesignation' обязательно для заполнения.")]
    [StringLength(150)]
    public string RequirementDesignation { get; set; } = string.Empty;

    [Required(ErrorMessage = "Поле 'Formulation' обязательно для заполнения.")]
    [StringLength(2000)]
    public string Formulation { get; set; } = string.Empty;

    [ValidateNever] 
    public Document Document { get; set; } = null!;
}