using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotByte.API.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Description", "ImageUrl", "IsActive", "MealType", "Name" },
                values: new object[,]
                {
                    { 1, "Start your day right with our delicious breakfast options", "https://images.unsplash.com/photo-1533089860892-a7c6f0a88666", true, "Breakfast", "Breakfast" },
                    { 2, "Juicy gourmet burgers with premium toppings", "https://images.unsplash.com/photo-1568901346375-23c9450c58cd", true, "All Day", "Burgers" },
                    { 3, "Handcrafted pizzas with fresh ingredients", "https://images.unsplash.com/photo-1565299624946-b28f40a0ae38", true, "All Day", "Pizza" },
                    { 4, "Authentic Middle Eastern flavors and grills", "https://images.unsplash.com/photo-1529006557810-274b9b2fc783", true, "Lunch", "Arabian" },
                    { 5, "Sweet treats to end your meal on a high note", "https://images.unsplash.com/photo-1551024506-0bccd828d307", true, "All Day", "Desserts" },
                    { 6, "Refreshing drinks and shakes", "https://images.unsplash.com/photo-1544145945-f90425340c7e", true, "All Day", "Beverages" }
                });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "RestaurantId", "CreatedAt", "Description", "Email", "ImageUrl", "IsActive", "Location", "Name", "PasswordHash", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Authentic South Indian & North Indian cuisine with a modern twist. Known for our aromatic biryanis and rich curries.", "contact@spicegarden.com", "https://images.unsplash.com/photo-1517248135467-4c7edcad34c4", true, "45 Anna Nagar, Chennai, Tamil Nadu 600040", "Spice Garden", "$2a$11$Fzqgehxgb0JDCA5IFnAVveKRN0fMIedQL8ZHh9J4bRYT9aonvV4R.", "9111222333" },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Gourmet burgers made with premium ingredients. Juicy patties, fresh veggies, and secret sauces.", "hello@burgerbarn.com", "https://images.unsplash.com/photo-1466978913421-dad2ebd01d17", true, "12 MG Road, Bangalore, Karnataka 560001", "Burger Barn", "$2a$11$.aNOyVy.pwvR.UD6kmLvhOwwLVBaaOG.yf6i7JGFQh8/WaP136bT6", "9222333444" },
                    { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Wood-fired pizzas with handmade dough, imported cheese, and fresh toppings. Italian flavors at your doorstep.", "order@pizzapalace.com", "https://images.unsplash.com/photo-1555396273-367ea4eb4db5", true, "88 Jubilee Hills, Hyderabad, Telangana 500033", "Pizza Palace", "$2a$11$IT/4YTel2ToRCybV2ByKl.ovCynR9UlsVvc2c6G5q3GYSkIwi8fgG", "9333444555" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$LFxh.zCNZaUWfxhf/tNRHu4YiCYk.VuVOt/8bmYCH4O308cZfmURO");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Address", "CreatedAt", "Email", "FirstName", "Gender", "IsActive", "LastName", "PasswordHash", "PhoneNumber", "Role" },
                values: new object[] { 2, "123 Main Street, Chennai", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "john@example.com", "John", "Male", true, "Doe", "$2a$11$R2KmvB4Ze6.CbBoEbRCqg.nD8tJottggzbn78gLkCwnZvMDTyQo/G", "9876543210", "User" });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "MenuItemId", "AvailableTime", "Calories", "Carbohydrates", "CategoryId", "CookingTime", "CreatedAt", "Description", "DiscountPrice", "Fats", "ImageUrl", "IsAvailable", "IsVeg", "Name", "Price", "Proteins", "RestaurantId", "TasteInfo" },
                values: new object[,]
                {
                    { 1, "Breakfast", 350, 55.0m, 1, 15, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Crispy rice crepe filled with spiced potato masala, served with sambar and coconut chutney", 99.00m, 12.0m, "https://images.unsplash.com/photo-1630383249896-424e482df921", true, true, "Masala Dosa", 120.00m, 8.5m, 1, "Spicy Light" },
                    { 2, "Breakfast", 280, 45.0m, 1, 10, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Soft steamed rice cakes with crispy medu vada, served with sambar and chutneys", null, 8.0m, "https://images.unsplash.com/photo-1589301760014-d929f3979dbc", true, true, "Idli Vada Combo", 90.00m, 7.0m, 1, "Sweet" },
                    { 3, "Lunch", 650, 60.0m, 4, 20, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tender marinated chicken wrapped in pita with garlic sauce, hummus, pickles, and fries", 189.00m, 28.0m, "https://images.unsplash.com/photo-1529006557810-274b9b2fc783", true, false, "Chicken Shawarma Plate", 220.00m, 35.0m, 1, "Spicy Light" },
                    { 4, "All Day", 480, 55.0m, 4, 15, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Crispy chickpea falafel in warm pita with tahini, fresh salad, and pickled vegetables", null, 20.0m, "https://images.unsplash.com/photo-1593001874117-c99c800e3eb7", true, true, "Falafel Wrap", 180.00m, 18.0m, 1, "Spicy Light" },
                    { 5, "All Day", 380, 58.0m, 5, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Soft milk-solid dumplings soaked in rose-flavored sugar syrup", null, 15.0m, "https://images.unsplash.com/photo-1666190073498-2a23c97508a0", true, true, "Gulab Jamun (4 pcs)", 100.00m, 5.0m, 1, "Sweet" },
                    { 6, "All Day", 220, 40.0m, 6, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Creamy yogurt blended with fresh Alphonso mango pulp and a hint of cardamom", null, 4.0m, "https://images.unsplash.com/photo-1626200419199-391ae4be7a41", true, true, "Mango Lassi", 80.00m, 6.0m, 1, "Sweet" },
                    { 7, "All Day", 720, 48.0m, 2, 18, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Double smashed beef patty with American cheese, caramelized onions, pickles, and secret sauce", 199.00m, 38.0m, "https://images.unsplash.com/photo-1568901346375-23c9450c58cd", true, false, "Classic Smash Burger", 249.00m, 42.0m, 2, "Spicy Light" },
                    { 8, "All Day", 850, 52.0m, 2, 20, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Grilled beef patty with crispy bacon, cheddar, onion rings, and smoky BBQ sauce", null, 45.0m, "https://images.unsplash.com/photo-1553979459-d2229ba7433b", true, false, "BBQ Bacon Burger", 299.00m, 48.0m, 2, "Spicy Full" },
                    { 9, "All Day", 520, 58.0m, 2, 15, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Crispy vegetable patty with lettuce, tomato, jalapeños, and chipotle mayo", 169.00m, 22.0m, "https://images.unsplash.com/photo-1520072959219-c595dc870360", true, true, "Veggie Supreme Burger", 199.00m, 18.0m, 2, "Spicy Light" },
                    { 10, "All Day", 450, 62.0m, 5, 12, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Warm chocolate cake with a molten chocolate center, served with vanilla ice cream", null, 22.0m, "https://images.unsplash.com/photo-1624353365286-3f8d62daad51", true, true, "Chocolate Lava Cake", 180.00m, 6.0m, 2, "Sweet" },
                    { 11, "All Day", 480, 68.0m, 6, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Thick and creamy milkshake loaded with crushed Oreo cookies and whipped cream", null, 18.0m, "https://images.unsplash.com/photo-1572490122747-3968b75cc699", true, true, "Oreo Milkshake", 150.00m, 10.0m, 2, "Sweet" },
                    { 12, "All Day", 280, 38.0m, 6, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Chilled coffee blended with milk, cream, and a touch of chocolate syrup", null, 10.0m, "https://images.unsplash.com/photo-1461023058943-07fcbe16d735", true, true, "Cold Coffee", 120.00m, 8.0m, 2, "Sweet" },
                    { 13, "All Day", 680, 85.0m, 3, 20, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Classic wood-fired pizza with San Marzano tomato sauce, fresh mozzarella, and basil", 249.00m, 22.0m, "https://images.unsplash.com/photo-1574071318508-1cdbab80d002", true, true, "Margherita Pizza", 299.00m, 28.0m, 3, "Sweet" },
                    { 14, "All Day", 820, 82.0m, 3, 22, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Loaded with spicy pepperoni, mozzarella, and our signature marinara sauce", 349.00m, 35.0m, "https://images.unsplash.com/photo-1628840042765-356cda07504e", true, false, "Pepperoni Pizza", 399.00m, 38.0m, 3, "Spicy Light" },
                    { 15, "All Day", 780, 78.0m, 3, 25, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Grilled chicken, red onions, bell peppers, and jalapeños on a smoky BBQ base", null, 30.0m, "https://images.unsplash.com/photo-1565299624946-b28f40a0ae38", true, false, "BBQ Chicken Pizza", 449.00m, 40.0m, 3, "Spicy Full" },
                    { 16, "All Day", 650, 80.0m, 3, 20, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Mushrooms, olives, capsicum, corn, onions, and tomatoes with extra cheese", 299.00m, 24.0m, "https://images.unsplash.com/photo-1571407970349-bc81e7e96d47", true, true, "Farm Fresh Veggie Pizza", 349.00m, 22.0m, 3, "Sweet" },
                    { 17, "All Day", 380, 48.0m, 5, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Classic Italian dessert with espresso-soaked ladyfingers and mascarpone cream", null, 18.0m, "https://images.unsplash.com/photo-1571877227200-a0d98ea607e9", true, true, "Tiramisu", 220.00m, 6.0m, 3, "Sweet" },
                    { 18, "All Day", 80, 20.0m, 6, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Freshly squeezed lime with soda, mint, and your choice of sweet or salted", null, 0.0m, "https://images.unsplash.com/photo-1513558161293-cdaf765ed514", true, true, "Fresh Lime Soda", 60.00m, 0.5m, 3, "Sweet" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "MenuItemId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$GSUtoI8aKJKf0vkCVGSJJujR/FtHTx229LJcSXkQtvH./yzips7E.");
        }
    }
}
