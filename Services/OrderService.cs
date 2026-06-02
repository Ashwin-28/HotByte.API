using HotByte.API.Data;
using HotByte.API.DTOs;
using HotByte.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HotByte.API.Services;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;

    public OrderService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OrderResponseDTO> PlaceOrderAsync(int userId, PlaceOrderDTO dto)
    {
        string resolvedAddress;

        if (dto.AddressId.HasValue && dto.NewAddress is not null)
        {
            throw new ArgumentException("Please provide either a saved Address ID or new address details, not both.");
        }

        if (dto.AddressId.HasValue)
        {
            var savedAddress = await _context.UserAddresses
                .FirstOrDefaultAsync(ua => ua.AddressId == dto.AddressId.Value && ua.UserId == userId && ua.IsActive)
                ?? throw new KeyNotFoundException($"Saved address with ID {dto.AddressId.Value} not found for this user.");

            resolvedAddress = $"{savedAddress.AddressLabel}: {savedAddress.AddressLine}, {savedAddress.City}, {savedAddress.State} - {savedAddress.PostalCode}";
        }
        else if (dto.NewAddress is not null)
        {
            var newAddress = new UserAddress
            {
                UserId = userId,
                AddressLabel = dto.NewAddress.AddressLabel,
                AddressLine = dto.NewAddress.AddressLine,
                City = dto.NewAddress.City,
                State = dto.NewAddress.State,
                PostalCode = dto.NewAddress.PostalCode,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.UserAddresses.Add(newAddress);
            await _context.SaveChangesAsync();

            resolvedAddress = $"{newAddress.AddressLabel}: {newAddress.AddressLine}, {newAddress.City}, {newAddress.State} - {newAddress.PostalCode}";
        }
        else
        {
            throw new ArgumentException("Please select a saved address or provide a new delivery address.");
        }

        var cart = await _context.Carts
            .Include(c => c.CartItems)
                .ThenInclude(ci => ci.MenuItem)
                    .ThenInclude(mi => mi.Restaurant)
            .FirstOrDefaultAsync(c => c.UserId == userId)
            ?? throw new InvalidOperationException("Cart is empty or not found.");

        if (!cart.CartItems.Any())
            throw new InvalidOperationException("Cart is empty.");

        // Group cart items by restaurant
        var groupedByRestaurant = cart.CartItems
            .GroupBy(ci => ci.MenuItem.RestaurantId);

        var orders = new List<Order>();

        foreach (var group in groupedByRestaurant)
        {
            var order = new Order
            {
                UserId = userId,
                RestaurantId = group.Key,
                DeliveryAddress = resolvedAddress,
                PaymentMethod = dto.PaymentMethod,
                TotalAmount = group.Sum(ci => ci.UnitPrice * ci.Quantity),
                OrderItems = group.Select(ci => new OrderItem
                {
                    MenuItemId = ci.MenuItemId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.UnitPrice,
                    ItemTotal = ci.UnitPrice * ci.Quantity
                }).ToList()
            };

            orders.Add(order);
        }

        _context.Orders.AddRange(orders);
        _context.CartItems.RemoveRange(cart.CartItems);
        await _context.SaveChangesAsync();

        // Return the first order (or primary order)
        var placedOrder = await _context.Orders
            .Include(o => o.Restaurant)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
            .FirstAsync(o => o.OrderId == orders.First().OrderId);

        return MapToResponse(placedOrder);
    }

    public async Task<IEnumerable<OrderResponseDTO>> GetUserOrdersAsync(int userId)
    {
        var orders = await _context.Orders
            .Include(o => o.Restaurant)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.PlacedAt)
            .ToListAsync();

        return orders.Select(MapToResponse);
    }

    public async Task<OrderResponseDTO?> GetOrderByIdAsync(int orderId, int userId)
    {
        var order = await _context.Orders
            .Include(o => o.Restaurant)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
            .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);

        return order is null ? null : MapToResponse(order);
    }

    public async Task<IEnumerable<OrderResponseDTO>> GetRestaurantOrdersAsync(int restaurantId)
    {
        var orders = await _context.Orders
            .Include(o => o.Restaurant)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
            .Where(o => o.RestaurantId == restaurantId)
            .OrderByDescending(o => o.PlacedAt)
            .ToListAsync();

        return orders.Select(MapToResponse);
    }

    public async Task<IEnumerable<OrderResponseDTO>> GetAllOrdersAsync()
    {
        var orders = await _context.Orders
            .Include(o => o.Restaurant)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
            .OrderByDescending(o => o.PlacedAt)
            .ToListAsync();

        return orders.Select(MapToResponse);
    }

    public async Task<OrderResponseDTO?> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDTO dto)
    {
        var order = await _context.Orders
            .Include(o => o.Restaurant)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);

        if (order is null) return null;

        order.Status = dto.Status;
        if (dto.EstimatedDelivery.HasValue)
            order.EstimatedDelivery = dto.EstimatedDelivery.Value;
        order.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return MapToResponse(order);
    }

    public async Task<bool> CancelOrderAsync(int orderId, int userId)
    {
        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);

        if (order is null || order.Status != "Pending") return false;

        order.Status = "Cancelled";
        order.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }

    private static OrderResponseDTO MapToResponse(Order order) => new(
        order.OrderId,
        order.Restaurant.Name,
        order.OrderItems.Select(oi => new OrderItemDTO(
            oi.OrderItemId,
            oi.MenuItemId,
            oi.MenuItem.Name,
            oi.Quantity,
            oi.UnitPrice,
            oi.ItemTotal
        )).ToList(),
        order.TotalAmount,
        order.DeliveryAddress,
        order.Status,
        order.PaymentMethod,
        order.PaymentStatus,
        order.EstimatedDelivery?.ToString("yyyy-MM-dd HH:mm:ss"),
        order.PlacedAt.ToString("yyyy-MM-dd HH:mm:ss")
    );
}
