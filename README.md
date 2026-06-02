# 🍔 HotByte — Online Food Delivery API

A **RESTful Web API** built with **ASP.NET Core (.NET 10)** that powers an online food delivery platform. The API handles the complete food ordering workflow — from browsing restaurants and menu items to managing carts, placing orders, and tracking order status.

---

## 📌 Project Overview

**HotByte** is a backend API for an online food delivery system (similar to Swiggy/Zomato). It enables:

- **Customers** to browse restaurants, search menus, add items to cart, and place orders
- **Restaurants** to manage their menu items and process incoming orders
- **Admins** to manage users, restaurants, and categories

The API follows a **clean layered architecture** with proper separation of concerns using the **Service Pattern**.

---

## 🛠️ Tech Stack

| Technology | Purpose |
|---|---|
| **ASP.NET Core (.NET 10)** | Web API framework |
| **Entity Framework Core 10** | ORM for database operations |
| **SQL Server (SQLEXPRESS)** | Relational database |
| **BCrypt.Net** | Password hashing |
| **JWT Bearer Authentication** | Token-based auth (planned) |
| **Swagger / Swashbuckle** | API documentation & testing UI |
| **C# Records** | Immutable DTOs |

---

## 🏗️ Architecture & Design Patterns

### Layered Architecture

```
┌──────────────────────────────────────────────────────────┐
│                    Controllers Layer                     │
│         (Handles HTTP requests & responses)              │
├──────────────────────────────────────────────────────────┤
│                    DTOs Layer                            │
│         (Data Transfer Objects for API contracts)        │
├──────────────────────────────────────────────────────────┤
│                    Services Layer                        │
│         (Business logic & data manipulation)             │
├──────────────────────────────────────────────────────────┤
│                    Data Layer                            │
│         (EF Core DbContext & database config)            │
├──────────────────────────────────────────────────────────┤
│                    Models Layer                          │
│         (Entity classes mapping to DB tables)            │
└──────────────────────────────────────────────────────────┘
```

### Design Patterns Used

| Pattern | Where Used |
|---|---|
| **Repository Pattern (via EF Core)** | `ApplicationDbContext` acts as the repository |
| **Service Pattern** | All business logic is in `Services/` with interfaces (`IAuthService`, `ICartService`, etc.) |
| **Dependency Injection** | Services registered in `Program.cs` and injected into controllers |
| **DTO Pattern** | Separate request/response models to decouple API from database entities |
| **Code-First Migrations** | Database schema managed through EF Core migrations |

---

## 🗄️ Database Design

### Entity Relationship Diagram

```
┌─────────────┐       ┌──────────────┐       ┌─────────────┐
│    User      │1────*│    Order      │*────1│  Restaurant  │
│─────────────│       │──────────────│       │─────────────│
│ UserId (PK) │       │ OrderId (PK) │       │RestaurantId │
│ FirstName   │       │ UserId (FK)  │       │ Name        │
│ LastName    │       │RestaurantId  │       │ Location    │
│ Email (UQ)  │       │ TotalAmount  │       │ Email (UQ)  │
│ PasswordHash│       │ DeliveryAddr │       │ PasswordHash│
│ PhoneNumber │       │ Status       │       │ Description │
│ Role        │       │ PaymentMethod│       │ ImageUrl    │
│ IsActive    │       │ PaymentStatus│       │ IsActive    │
└──────┬──────┘       │ PlacedAt     │       └──────┬──────┘
       │              └──────┬───────┘              │
       │                     │                      │
       │1                    │1                     │1
       │                     │                      │
       ▼*                    ▼*                     ▼*
┌─────────────┐       ┌──────────────┐       ┌─────────────┐
│ UserAddress  │       │  OrderItem   │       │  MenuItem   │
│─────────────│       │──────────────│       │─────────────│
│ AddressId   │       │OrderItemId   │       │MenuItemId   │
│ UserId (FK) │       │ OrderId (FK) │       │RestaurantId │
│ AddressLabel│       │MenuItemId(FK)│       │ CategoryId  │
│ AddressLine │       │ Quantity     │       │ Name        │
│ City        │       │ UnitPrice    │       │ Price       │
│ State       │       │ ItemTotal    │       │DiscountPrice│
│ PostalCode  │       └──────────────┘       │ IsVeg       │
└─────────────┘                              │ Calories    │
                                             │ IsAvailable │
       ┌─────────────┐                      └─────────────┘
       │   Cart      │                            ▲
       │─────────────│                            │
       │ CartId (PK) │       ┌──────────────┐     │
       │ UserId (FK) │1────*│  CartItem     │*────┘
       │ CreatedAt   │       │──────────────│
       │ UpdatedAt   │       │CartItemId    │  ┌─────────────┐
       └─────────────┘       │ CartId (FK)  │  │  Category   │
                             │MenuItemId(FK)│  │─────────────│
                             │ Quantity     │  │CategoryId   │
                             │ UnitPrice    │  │ Name        │
                             └──────────────┘  │ MealType    │
                                               │ Description │
                                               │ ImageUrl    │
                                               └─────────────┘
```

### Key Relationships

| Relationship | Type | Delete Behavior |
|---|---|---|
| User → Cart | One-to-One | Cascade |
| User → Orders | One-to-Many | Cascade |
| User → UserAddresses | One-to-Many | Cascade |
| Restaurant → MenuItems | One-to-Many | Cascade |
| Restaurant → Orders | One-to-Many | Restrict |
| Category → MenuItems | One-to-Many | Restrict |
| Cart → CartItems | One-to-Many | Cascade |
| Order → OrderItems | One-to-Many | Cascade |
| CartItem → MenuItem | Many-to-One | Restrict |
| OrderItem → MenuItem | Many-to-One | Restrict |

---

## 📡 API Endpoints

All endpoints are prefixed with `/api/v1/`

### 🔐 Authentication (`/api/v1/auth`)

| Method | Endpoint | Description | Status |
|---|---|---|---|
| `POST` | `/register` | Register a new user | 🔜 Planned |
| `POST` | `/login` | Login and get JWT token | 🔜 Planned |

### 👤 Users (`/api/v1/users`)

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/profile` | Get current user's profile |
| `PUT` | `/profile` | Update current user's profile |
| `GET` | `/` | Get all users (Admin) |
| `DELETE` | `/{id}` | Delete a user (Admin) |
| `GET` | `/addresses` | Get user's saved addresses |
| `POST` | `/addresses` | Add a new address |
| `DELETE` | `/addresses/{id}` | Delete an address |

### 🏪 Restaurants (`/api/v1/restaurants`)

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/` | Get all restaurants |
| `GET` | `/{id}` | Get restaurant by ID |
| `POST` | `/` | Create a restaurant (Admin) |
| `PUT` | `/{id}` | Update a restaurant |
| `DELETE` | `/{id}` | Delete a restaurant (Admin) |

### 📂 Categories (`/api/v1/categories`)

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/` | Get all categories |
| `GET` | `/{id}` | Get category by ID |
| `POST` | `/` | Create a category (Admin) |
| `PUT` | `/{id}` | Update a category (Admin) |
| `DELETE` | `/{id}` | Delete a category (Admin) |

### 🍕 Menu Items (`/api/v1/menuitems`)

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/` | Get all menu items |
| `GET` | `/{id}` | Get menu item by ID |
| `GET` | `/restaurant/{id}` | Get items by restaurant |
| `GET` | `/category/{id}` | Get items by category |
| `GET` | `/search?q=` | Search menu items by name |
| `POST` | `/` | Create a menu item |
| `PUT` | `/{id}` | Update a menu item |
| `PATCH` | `/{id}/availability` | Toggle item availability |
| `DELETE` | `/{id}` | Delete a menu item |

### 🛒 Cart (`/api/v1/cart`)

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/` | Get current user's cart |
| `POST` | `/add` | Add item to cart |
| `PUT` | `/update/{cartItemId}` | Update cart item quantity |
| `DELETE` | `/remove/{cartItemId}` | Remove item from cart |
| `DELETE` | `/clear` | Clear entire cart |

### 📦 Orders (`/api/v1/orders`)

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/` | Place a new order (from cart) |
| `GET` | `/` | Get current user's orders |
| `GET` | `/{id}` | Get order by ID |
| `GET` | `/restaurant` | Get restaurant's orders |
| `PATCH` | `/{id}/status` | Update order status |
| `GET` | `/all` | Get all orders (Admin) |
| `DELETE` | `/{id}` | Cancel an order |

---

## 📁 Project Structure

```
HotByte.API/
├── Controllers/                  # API endpoint handlers
│   ├── AuthController.cs         # Authentication (register/login)
│   ├── CartController.cs         # Cart management
│   ├── CategoriesController.cs   # Food categories CRUD
│   ├── MenuItemsController.cs    # Menu items CRUD + search
│   ├── OrdersController.cs       # Order placement & tracking
│   ├── RestaurantsController.cs  # Restaurant CRUD
│   └── UsersController.cs        # User profiles & addresses
│
├── DTOs/                         # Data Transfer Objects
│   ├── AuthDTOs.cs               # RegisterUserDTO, LoginDTO, AuthResponseDTO
│   ├── CartDTOs.cs               # AddToCartDTO, UpdateCartItemDTO
│   ├── MenuItemDTOs.cs           # CreateMenuItemDTO
│   ├── OrderDTOs.cs              # PlaceOrderDTO, OrderResponseDTO
│   ├── RestaurantDTOs.cs         # CreateRestaurantDTO
│   ├── UserAddressDTOs.cs        # CreateUserAddressDTO
│   └── UserDTOs.cs               # UpdateUserDTO
│
├── Data/                         # Database layer
│   └── ApplicationDbContext.cs   # EF Core context, Fluent API config, seed data
│
├── Migrations/                   # EF Core database migrations
│   ├── InitialCreate             # Initial schema creation
│   ├── AddSeedData               # Seed data for restaurants, menus, etc.
│   └── AddUserAddressSupport     # User address feature migration
│
├── Models/                       # Entity / domain models
│   ├── Cart.cs                   # Shopping cart entity
│   ├── CartItem.cs               # Individual cart item
│   ├── Category.cs               # Food category (Breakfast, Pizza, etc.)
│   ├── MenuItem.cs               # Menu item with nutrition info
│   ├── Order.cs                  # Customer order
│   ├── OrderItem.cs              # Individual order line item
│   ├── Restaurant.cs             # Restaurant entity
│   ├── User.cs                   # User entity with roles
│   └── UserAddress.cs            # User's saved delivery addresses
│
├── Services/                     # Business logic layer
│   ├── IAuthService.cs           # Auth service interface
│   ├── AuthService.cs            # Auth service implementation
│   ├── ICartService.cs           # Cart service interface
│   ├── CartService.cs            # Cart service implementation
│   ├── ICategoryService.cs       # Category service interface
│   ├── CategoryService.cs        # Category service implementation
│   ├── IMenuItemService.cs       # MenuItem service interface
│   ├── MenuItemService.cs        # MenuItem service implementation
│   ├── IOrderService.cs          # Order service interface
│   ├── OrderService.cs           # Order service implementation
│   ├── IRestaurantService.cs     # Restaurant service interface
│   ├── RestaurantService.cs      # Restaurant service implementation
│   ├── IUserService.cs           # User service interface
│   └── UserService.cs            # User service implementation
│
├── Properties/
│   └── launchSettings.json       # Development launch configuration
│
├── Program.cs                    # Application entry point & DI configuration
├── appsettings.json              # App configuration (DB connection, JWT settings)
├── HotByte.API.csproj            # Project file with NuGet dependencies
└── README.md                     # This file
```

---

## ⚙️ How to Run

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB or SQLEXPRESS)

### Steps

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd HotByte.API
   ```

2. **Update the connection string** (if needed)
   
   Edit `appsettings.json` and update the `DefaultConnection` to match your SQL Server instance:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Data Source=(local)\\SQLEXPRESS;Initial Catalog=SoundProject;Integrated Security=True;Trust Server Certificate=True"
   }
   ```

3. **Apply database migrations**
   ```bash
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Open Swagger UI** to explore and test the API:
   ```
   https://localhost:<port>/swagger
   ```

### Seed Data

The database comes pre-seeded with:

| Entity | Count | Examples |
|---|---|---|
| Users | 2 | Admin user, sample customer (John Doe) |
| Restaurants | 3 | Spice Garden, Burger Barn, Pizza Palace |
| Categories | 6 | Breakfast, Burgers, Pizza, Arabian, Desserts, Beverages |
| Menu Items | 18 | Masala Dosa, Classic Smash Burger, Margherita Pizza, etc. |
| User Addresses | 1 | Sample home address for John Doe |

---

## 🔄 Order Workflow

```
  Customer browses          Customer adds items         Customer places
  restaurants & menus  ──▶  to shopping cart       ──▶  order from cart
        │                        │                          │
        ▼                        ▼                          ▼
  GET /restaurants          POST /cart/add             POST /orders
  GET /menuitems            PUT  /cart/update          (Cart → Order)
  GET /menuitems/search     DEL  /cart/remove
        │                                                   │
        │                                                   ▼
        │                                          Order Status Flow:
        │                                    ┌──────────────────────┐
        │                                    │  Pending             │
        │                                    │    ↓                 │
        │                                    │  Confirmed           │
        │                                    │    ↓                 │
        │                                    │  Preparing           │
        │                                    │    ↓                 │
        │                                    │  Out for Delivery    │
        │                                    │    ↓                 │
        │                                    │  Delivered           │
        │                                    └──────────────────────┘
        │                                    (or Cancelled at any
        │                                     Pending stage)
        │
        ▼
  Restaurant updates
  order status via:
  PATCH /orders/{id}/status
```

---

## 🔒 Authentication (Current & Planned)

### Current State
- Auth endpoints (`/register`, `/login`) are **stubbed** — they return `501 Not Implemented`
- User identification uses a **temporary header-based approach**: `X-User-Id` and `X-Restaurant-Id` headers
- JWT packages (`Microsoft.AspNetCore.Authentication.JwtBearer`) and settings are already configured

### Planned Implementation
- **BCrypt** password hashing (package already installed)
- **JWT Bearer token** authentication
- **Role-based authorization** (User, Admin, Restaurant roles)
- Replace header-based user identification with JWT claims

---

## 🧪 Testing the API

### Using Swagger UI
1. Run the project and navigate to `/swagger`
2. Use the Swagger UI to send requests to all endpoints
3. For endpoints requiring a user context, add the `X-User-Id` header (e.g., `2` for John Doe)
4. For restaurant-specific endpoints, add `X-Restaurant-Id` header (e.g., `1` for Spice Garden)

### Sample Request Flow

```
1. GET  /api/v1/restaurants              → Browse all restaurants
2. GET  /api/v1/menuitems/restaurant/1   → View Spice Garden's menu
3. POST /api/v1/cart/add                 → Add "Masala Dosa" to cart
       Headers: X-User-Id: 2
       Body: { "menuItemId": 1, "quantity": 2 }
4. GET  /api/v1/cart                     → View cart contents
       Headers: X-User-Id: 2
5. POST /api/v1/orders                  → Place the order
       Headers: X-User-Id: 2
       Body: { "addressId": 1, "paymentMethod": "UPI" }
6. GET  /api/v1/orders                  → View order history
       Headers: X-User-Id: 2
```

---

## 📦 NuGet Packages

| Package | Version | Purpose |
|---|---|---|
| `BCrypt.Net-Next` | 4.2.0 | Secure password hashing |
| `Microsoft.AspNetCore.Authentication.JwtBearer` | 10.0.8 | JWT authentication support |
| `Microsoft.EntityFrameworkCore` | 10.0.8 | ORM framework |
| `Microsoft.EntityFrameworkCore.SqlServer` | 10.0.8 | SQL Server database provider |
| `Microsoft.EntityFrameworkCore.Design` | 10.0.8 | Design-time EF Core tools |
| `Microsoft.EntityFrameworkCore.Tools` | 10.0.8 | CLI migration tools |
| `Swashbuckle.AspNetCore` | 6.5.0 | Swagger/OpenAPI documentation |

---

## 🚀 Future Enhancements

- [ ] Complete JWT authentication & authorization
- [ ] Role-based access control (Admin, Restaurant Owner, Customer)
- [ ] Payment gateway integration
- [ ] Real-time order tracking with SignalR
- [ ] Image upload for menu items & restaurants
- [ ] Rating & review system
- [ ] Push notifications for order updates
- [ ] Pagination & filtering for large data sets
- [ ] Unit tests & integration tests

---

## 👨‍💻 Author

**HotByte Team**

---

> **Note:** This project is currently in active development. Authentication endpoints are stubbed and will be implemented in future iterations.
