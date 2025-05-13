using System.ComponentModel.DataAnnotations;

namespace ch_12_auto_mapper.Entities;

public class Book
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public decimal Price { get; set; }
}