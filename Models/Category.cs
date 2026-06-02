using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotByte.API.Models;

public class Category
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CategoryId { get; set; }

    [Required, MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? MealType { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
}
