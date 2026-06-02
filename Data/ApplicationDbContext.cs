using HotByte.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HotByte.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Restaurant> Restaurants => Set<Restaurant>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<UserAddress> UserAddresses => Set<UserAddress>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User - unique email
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Restaurant - unique email
        modelBuilder.Entity<Restaurant>()
            .HasIndex(r => r.Email)
            .IsUnique();

        // Cart - one cart per user
        modelBuilder.Entity<Cart>()
            .HasIndex(c => c.UserId)
            .IsUnique();

        // Cart -> User relationship
        modelBuilder.Entity<Cart>()
            .HasOne(c => c.User)
            .WithOne(u => u.Cart)
            .HasForeignKey<Cart>(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // CartItem -> Cart
        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Cart)
            .WithMany(c => c.CartItems)
            .HasForeignKey(ci => ci.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        // CartItem -> MenuItem
        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.MenuItem)
            .WithMany()
            .HasForeignKey(ci => ci.MenuItemId)
            .OnDelete(DeleteBehavior.Restrict);

        // MenuItem -> Restaurant
        modelBuilder.Entity<MenuItem>()
            .HasOne(mi => mi.Restaurant)
            .WithMany(r => r.MenuItems)
            .HasForeignKey(mi => mi.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);

        // MenuItem -> Category
        modelBuilder.Entity<MenuItem>()
            .HasOne(mi => mi.Category)
            .WithMany(c => c.MenuItems)
            .HasForeignKey(mi => mi.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Order -> User
        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Order -> Restaurant
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Restaurant)
            .WithMany(r => r.Orders)
            .HasForeignKey(o => o.RestaurantId)
            .OnDelete(DeleteBehavior.Restrict);

        // OrderItem -> Order
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // OrderItem -> MenuItem
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.MenuItem)
            .WithMany()
            .HasForeignKey(oi => oi.MenuItemId)
            .OnDelete(DeleteBehavior.Restrict);

        // UserAddress -> User
        modelBuilder.Entity<UserAddress>()
            .HasOne(ua => ua.User)
            .WithMany(u => u.SavedAddresses)
            .HasForeignKey(ua => ua.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // ───── SEED DATA ─────

        var seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // Seed Admin user
        modelBuilder.Entity<User>().HasData(new User
        {
            UserId = 1,
            FirstName = "Admin",
            LastName = "User",
            Email = "admin@hotbyte.com",
            PasswordHash = "$2a$11$APYGzGiIm7dO4oWMit/ACOOuvPNcsRSI6dYlTnq3Mz0Zyo3McMdpa",
            PhoneNumber = "0000000000",
            Role = "Admin",
            IsActive = true,
            CreatedAt = seedDate
        });

        // Seed sample User
        modelBuilder.Entity<User>().HasData(new User
        {
            UserId = 2,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            PasswordHash = "$2a$11$75yFIf0UbgpoUlb5e9biuOF.pBpl79K6oK6VtRGP/0/gwiENyTKje",
            PhoneNumber = "9876543210",
            Gender = "Male",
            Address = "123 Main Street, Chennai",
            Role = "User",
            IsActive = true,
            CreatedAt = seedDate
        });

        // Seed sample UserAddress
        modelBuilder.Entity<UserAddress>().HasData(new UserAddress
        {
            AddressId = 1,
            UserId = 2,
            AddressLabel = "Home",
            AddressLine = "123 Main Street",
            City = "Chennai",
            State = "Tamil Nadu",
            PostalCode = "600001",
            IsActive = true,
            CreatedAt = seedDate
        });

        // ───── Restaurants ─────
        modelBuilder.Entity<Restaurant>().HasData(
            new Restaurant
            {
                RestaurantId = 1,
                Name = "Spice Garden",
                Location = "45 Anna Nagar, Chennai, Tamil Nadu 600040",
                PhoneNumber = "9111222333",
                Email = "contact@spicegarden.com",
                PasswordHash = "$2a$11$Ie..HR8H5/6YIpBb4et6P.TwHp62a4NBuWfp7wG.akenaTpHPpljW",
                Description = "Authentic South Indian & North Indian cuisine with a modern twist. Known for our aromatic biryanis and rich curries.",
                ImageUrl = "https://images.unsplash.com/photo-1517248135467-4c7edcad34c4",
                IsActive = true,
                CreatedAt = seedDate
            },
            new Restaurant
            {
                RestaurantId = 2,
                Name = "Burger Barn",
                Location = "12 MG Road, Bangalore, Karnataka 560001",
                PhoneNumber = "9222333444",
                Email = "hello@burgerbarn.com",
                PasswordHash = "$2a$11$Ie..HR8H5/6YIpBb4et6P.TwHp62a4NBuWfp7wG.akenaTpHPpljW",
                Description = "Gourmet burgers made with premium ingredients. Juicy patties, fresh veggies, and secret sauces.",
                ImageUrl = "https://images.unsplash.com/photo-1466978913421-dad2ebd01d17",
                IsActive = true,
                CreatedAt = seedDate
            },
            new Restaurant
            {
                RestaurantId = 3,
                Name = "Pizza Palace",
                Location = "88 Jubilee Hills, Hyderabad, Telangana 500033",
                PhoneNumber = "9333444555",
                Email = "order@pizzapalace.com",
                PasswordHash = "$2a$11$Ie..HR8H5/6YIpBb4et6P.TwHp62a4NBuWfp7wG.akenaTpHPpljW",
                Description = "Wood-fired pizzas with handmade dough, imported cheese, and fresh toppings. Italian flavors at your doorstep.",
                ImageUrl = "https://images.unsplash.com/photo-1555396273-367ea4eb4db5",
                IsActive = true,
                CreatedAt = seedDate
            }
        );

        // ───── Categories ─────
        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                CategoryId = 1,
                Name = "Breakfast",
                Description = "Start your day right with our delicious breakfast options",
                MealType = "Breakfast",
                ImageUrl = "https://images.unsplash.com/photo-1533089860892-a7c6f0a88666",
                IsActive = true
            },
            new Category
            {
                CategoryId = 2,
                Name = "Burgers",
                Description = "Juicy gourmet burgers with premium toppings",
                MealType = "All Day",
                ImageUrl = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd",
                IsActive = true
            },
            new Category
            {
                CategoryId = 3,
                Name = "Pizza",
                Description = "Handcrafted pizzas with fresh ingredients",
                MealType = "All Day",
                ImageUrl = "https://images.unsplash.com/photo-1565299624946-b28f40a0ae38",
                IsActive = true
            },
            new Category
            {
                CategoryId = 4,
                Name = "Arabian",
                Description = "Authentic Middle Eastern flavors and grills",
                MealType = "Lunch",
                ImageUrl = "https://images.unsplash.com/photo-1529006557810-274b9b2fc783",
                IsActive = true
            },
            new Category
            {
                CategoryId = 5,
                Name = "Desserts",
                Description = "Sweet treats to end your meal on a high note",
                MealType = "All Day",
                ImageUrl = "https://images.unsplash.com/photo-1551024506-0bccd828d307",
                IsActive = true
            },
            new Category
            {
                CategoryId = 6,
                Name = "Beverages",
                Description = "Refreshing drinks and shakes",
                MealType = "All Day",
                ImageUrl = "https://images.unsplash.com/photo-1544145945-f90425340c7e",
                IsActive = true
            }
        );

        // ───── Menu Items ─────
        modelBuilder.Entity<MenuItem>().HasData(
            // ── Spice Garden (RestaurantId = 1) ──
            new MenuItem
            {
                MenuItemId = 1, RestaurantId = 1, CategoryId = 1,
                Name = "Masala Dosa",
                Description = "Crispy rice crepe filled with spiced potato masala, served with sambar and coconut chutney",
                Price = 120.00m, DiscountPrice = 99.00m,
                IsVeg = true, TasteInfo = "Spicy Light",
                Calories = 350, Proteins = 8.5m, Fats = 12.0m, Carbohydrates = 55.0m,
                CookingTime = 15, AvailableTime = "Breakfast",
                ImageUrl = "https://images.unsplash.com/photo-1630383249896-424e482df921",
                IsAvailable = true, CreatedAt = seedDate
            },
            new MenuItem
            {
                MenuItemId = 2, RestaurantId = 1, CategoryId = 1,
                Name = "Idli Vada Combo",
                Description = "Soft steamed rice cakes with crispy medu vada, served with sambar and chutneys",
                Price = 90.00m,
                IsVeg = true, TasteInfo = "Sweet",
                Calories = 280, Proteins = 7.0m, Fats = 8.0m, Carbohydrates = 45.0m,
                CookingTime = 10, AvailableTime = "Breakfast",
                ImageUrl = "https://images.unsplash.com/photo-1589301760014-d929f3979dbc",
                IsAvailable = true, CreatedAt = seedDate
            },
            new MenuItem
            {
                MenuItemId = 3, RestaurantId = 1, CategoryId = 4,
                Name = "Chicken Shawarma Plate",
                Description = "Tender marinated chicken wrapped in pita with garlic sauce, hummus, pickles, and fries",
                Price = 220.00m, DiscountPrice = 189.00m,
                IsVeg = false, TasteInfo = "Spicy Light",
                Calories = 650, Proteins = 35.0m, Fats = 28.0m, Carbohydrates = 60.0m,
                CookingTime = 20, AvailableTime = "Lunch",
                ImageUrl = "https://images.unsplash.com/photo-1529006557810-274b9b2fc783",
                IsAvailable = true, CreatedAt = seedDate
            },
            new MenuItem
            {
                MenuItemId = 4, RestaurantId = 1, CategoryId = 4,
                Name = "Falafel Wrap",
                Description = "Crispy chickpea falafel in warm pita with tahini, fresh salad, and pickled vegetables",
                Price = 180.00m,
                IsVeg = true, TasteInfo = "Spicy Light",
                Calories = 480, Proteins = 18.0m, Fats = 20.0m, Carbohydrates = 55.0m,
                CookingTime = 15, AvailableTime = "All Day",
                ImageUrl = "https://images.unsplash.com/photo-1593001874117-c99c800e3eb7",
                IsAvailable = true, CreatedAt = seedDate
            },
            new MenuItem
            {
                MenuItemId = 5, RestaurantId = 1, CategoryId = 5,
                Name = "Gulab Jamun (4 pcs)",
                Description = "Soft milk-solid dumplings soaked in rose-flavored sugar syrup",
                Price = 100.00m,
                IsVeg = true, TasteInfo = "Sweet",
                Calories = 380, Proteins = 5.0m, Fats = 15.0m, Carbohydrates = 58.0m,
                CookingTime = 5, AvailableTime = "All Day",
                ImageUrl = "https://images.unsplash.com/photo-1666190073498-2a23c97508a0",
                IsAvailable = true, CreatedAt = seedDate
            },
            new MenuItem
            {
                MenuItemId = 6, RestaurantId = 1, CategoryId = 6,
                Name = "Mango Lassi",
                Description = "Creamy yogurt blended with fresh Alphonso mango pulp and a hint of cardamom",
                Price = 80.00m,
                IsVeg = true, TasteInfo = "Sweet",
                Calories = 220, Proteins = 6.0m, Fats = 4.0m, Carbohydrates = 40.0m,
                CookingTime = 5, AvailableTime = "All Day",
                ImageUrl = "https://images.unsplash.com/photo-1626200419199-391ae4be7a41",
                IsAvailable = true, CreatedAt = seedDate
            },

            // ── Burger Barn (RestaurantId = 2) ──
            new MenuItem
            {
                MenuItemId = 7, RestaurantId = 2, CategoryId = 2,
                Name = "Classic Smash Burger",
                Description = "Double smashed beef patty with American cheese, caramelized onions, pickles, and secret sauce",
                Price = 249.00m, DiscountPrice = 199.00m,
                IsVeg = false, TasteInfo = "Spicy Light",
                Calories = 720, Proteins = 42.0m, Fats = 38.0m, Carbohydrates = 48.0m,
                CookingTime = 18, AvailableTime = "All Day",
                ImageUrl = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd",
                IsAvailable = true, CreatedAt = seedDate
            },
            new MenuItem
            {
                MenuItemId = 8, RestaurantId = 2, CategoryId = 2,
                Name = "BBQ Bacon Burger",
                Description = "Grilled beef patty with crispy bacon, cheddar, onion rings, and smoky BBQ sauce",
                Price = 299.00m,
                IsVeg = false, TasteInfo = "Spicy Full",
                Calories = 850, Proteins = 48.0m, Fats = 45.0m, Carbohydrates = 52.0m,
                CookingTime = 20, AvailableTime = "All Day",
                ImageUrl = "https://images.unsplash.com/photo-1553979459-d2229ba7433b",
                IsAvailable = true, CreatedAt = seedDate
            },
            new MenuItem
            {
                MenuItemId = 9, RestaurantId = 2, CategoryId = 2,
                Name = "Veggie Supreme Burger",
                Description = "Crispy vegetable patty with lettuce, tomato, jalapeños, and chipotle mayo",
                Price = 199.00m, DiscountPrice = 169.00m,
                IsVeg = true, TasteInfo = "Spicy Light",
                Calories = 520, Proteins = 18.0m, Fats = 22.0m, Carbohydrates = 58.0m,
                CookingTime = 15, AvailableTime = "All Day",
                ImageUrl = "https://images.unsplash.com/photo-1520072959219-c595dc870360",
                IsAvailable = true, CreatedAt = seedDate
            },
            new MenuItem
            {
                MenuItemId = 10, RestaurantId = 2, CategoryId = 5,
                Name = "Chocolate Lava Cake",
                Description = "Warm chocolate cake with a molten chocolate center, served with vanilla ice cream",
                Price = 180.00m,
                IsVeg = true, TasteInfo = "Sweet",
                Calories = 450, Proteins = 6.0m, Fats = 22.0m, Carbohydrates = 62.0m,
                CookingTime = 12, AvailableTime = "All Day",
                ImageUrl = "https://images.unsplash.com/photo-1624353365286-3f8d62daad51",
                IsAvailable = true, CreatedAt = seedDate
            },
            new MenuItem
            {
                MenuItemId = 11, RestaurantId = 2, CategoryId = 6,
                Name = "Oreo Milkshake",
                Description = "Thick and creamy milkshake loaded with crushed Oreo cookies and whipped cream",
                Price = 150.00m,
                IsVeg = true, TasteInfo = "Sweet",
                Calories = 480, Proteins = 10.0m, Fats = 18.0m, Carbohydrates = 68.0m,
                CookingTime = 5, AvailableTime = "All Day",
                ImageUrl = "https://images.unsplash.com/photo-1572490122747-3968b75cc699",
                IsAvailable = true, CreatedAt = seedDate
            },
            new MenuItem
            {
                MenuItemId = 12, RestaurantId = 2, CategoryId = 6,
                Name = "Cold Coffee",
                Description = "Chilled coffee blended with milk, cream, and a touch of chocolate syrup",
                Price = 120.00m,
                IsVeg = true, TasteInfo = "Sweet",
                Calories = 280, Proteins = 8.0m, Fats = 10.0m, Carbohydrates = 38.0m,
                CookingTime = 5, AvailableTime = "All Day",
                ImageUrl = "https://images.unsplash.com/photo-1461023058943-07fcbe16d735",
                IsAvailable = true, CreatedAt = seedDate
            },

            // ── Pizza Palace (RestaurantId = 3) ──
            new MenuItem
            {
                MenuItemId = 13, RestaurantId = 3, CategoryId = 3,
                Name = "Margherita Pizza",
                Description = "Classic wood-fired pizza with San Marzano tomato sauce, fresh mozzarella, and basil",
                Price = 299.00m, DiscountPrice = 249.00m,
                IsVeg = true, TasteInfo = "Sweet",
                Calories = 680, Proteins = 28.0m, Fats = 22.0m, Carbohydrates = 85.0m,
                CookingTime = 20, AvailableTime = "All Day",
                ImageUrl = "https://images.unsplash.com/photo-1574071318508-1cdbab80d002",
                IsAvailable = true, CreatedAt = seedDate
            },
            new MenuItem
            {
                MenuItemId = 14, RestaurantId = 3, CategoryId = 3,
                Name = "Pepperoni Pizza",
                Description = "Loaded with spicy pepperoni, mozzarella, and our signature marinara sauce",
                Price = 399.00m, DiscountPrice = 349.00m,
                IsVeg = false, TasteInfo = "Spicy Light",
                Calories = 820, Proteins = 38.0m, Fats = 35.0m, Carbohydrates = 82.0m,
                CookingTime = 22, AvailableTime = "All Day",
                ImageUrl = "https://images.unsplash.com/photo-1628840042765-356cda07504e",
                IsAvailable = true, CreatedAt = seedDate
            },
            new MenuItem
            {
                MenuItemId = 15, RestaurantId = 3, CategoryId = 3,
                Name = "BBQ Chicken Pizza",
                Description = "Grilled chicken, red onions, bell peppers, and jalapeños on a smoky BBQ base",
                Price = 449.00m,
                IsVeg = false, TasteInfo = "Spicy Full",
                Calories = 780, Proteins = 40.0m, Fats = 30.0m, Carbohydrates = 78.0m,
                CookingTime = 25, AvailableTime = "All Day",
                ImageUrl = "https://images.unsplash.com/photo-1565299624946-b28f40a0ae38",
                IsAvailable = true, CreatedAt = seedDate
            },
            new MenuItem
            {
                MenuItemId = 16, RestaurantId = 3, CategoryId = 3,
                Name = "Farm Fresh Veggie Pizza",
                Description = "Mushrooms, olives, capsicum, corn, onions, and tomatoes with extra cheese",
                Price = 349.00m, DiscountPrice = 299.00m,
                IsVeg = true, TasteInfo = "Sweet",
                Calories = 650, Proteins = 22.0m, Fats = 24.0m, Carbohydrates = 80.0m,
                CookingTime = 20, AvailableTime = "All Day",
                ImageUrl = "https://images.unsplash.com/photo-1571407970349-bc81e7e96d47",
                IsAvailable = true, CreatedAt = seedDate
            },
            new MenuItem
            {
                MenuItemId = 17, RestaurantId = 3, CategoryId = 5,
                Name = "Tiramisu",
                Description = "Classic Italian dessert with espresso-soaked ladyfingers and mascarpone cream",
                Price = 220.00m,
                IsVeg = true, TasteInfo = "Sweet",
                Calories = 380, Proteins = 6.0m, Fats = 18.0m, Carbohydrates = 48.0m,
                CookingTime = 5, AvailableTime = "All Day",
                ImageUrl = "https://images.unsplash.com/photo-1571877227200-a0d98ea607e9",
                IsAvailable = true, CreatedAt = seedDate
            },
            new MenuItem
            {
                MenuItemId = 18, RestaurantId = 3, CategoryId = 6,
                Name = "Fresh Lime Soda",
                Description = "Freshly squeezed lime with soda, mint, and your choice of sweet or salted",
                Price = 60.00m,
                IsVeg = true, TasteInfo = "Sweet",
                Calories = 80, Proteins = 0.5m, Fats = 0.0m, Carbohydrates = 20.0m,
                CookingTime = 3, AvailableTime = "All Day",
                ImageUrl = "https://images.unsplash.com/photo-1513558161293-cdaf765ed514",
                IsAvailable = true, CreatedAt = seedDate
            }
        );
    }
}
