using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotByte.API.Models;

public class CartItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CartItemId { get; set; }

    [Required]
    public int CartId { get; set; }

    [Required]
    public int MenuItemId { get; set; }

    [Required, Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal UnitPrice { get; set; }

    // Navigation
    [ForeignKey("CartId")]
    public Cart Cart { get; set; } = null!;

    [ForeignKey("MenuItemId")]
    public MenuItem MenuItem { get; set; } = null!;
}
