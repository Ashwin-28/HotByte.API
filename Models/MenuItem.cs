using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotByte.API.Models;

public class MenuItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MenuItemId { get; set; }

    [Required]
    public int RestaurantId { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? DiscountPrice { get; set; }

    public string? ImageUrl { get; set; }

    [Required]
    public bool IsVeg { get; set; }

    public string? TasteInfo { get; set; }

    public int? Calories { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Proteins { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Fats { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Carbohydrates { get; set; }

    public int? CookingTime { get; set; }

    public string? AvailableTime { get; set; }

    public bool IsAvailable { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    [ForeignKey("RestaurantId")]
    public Restaurant Restaurant { get; set; } = null!;

    [ForeignKey("CategoryId")]
    public Category Category { get; set; } = null!;
}
