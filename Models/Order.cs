using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotByte.API.Models;

public class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int RestaurantId { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [Required]
    public string DeliveryAddress { get; set; } = string.Empty;

    public string Status { get; set; } = "Pending";

    [Required]
    public string PaymentMethod { get; set; } = string.Empty;

    public string PaymentStatus { get; set; } = "Pending";

    public DateTime? EstimatedDelivery { get; set; }

    public DateTime PlacedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    [ForeignKey("UserId")]
    public User User { get; set; } = null!;

    [ForeignKey("RestaurantId")]
    public Restaurant Restaurant { get; set; } = null!;

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
