using System.ComponentModel.DataAnnotations;

namespace ch_15_identity.Entities.DTOs;

public abstract record BookDtoBase
{
    [MinLength(2,ErrorMessage = "Min. len must be 2")]
    [MaxLength(25,ErrorMessage = "Max. len must be 25")]
    public string? Title { get; init; }
    [Range(10,100)]
    public decimal Price { get; init; }
}