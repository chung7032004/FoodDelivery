using System.Reflection;
using PermissionConstants = FoodDelivery.Constants.Permissions;
using FoodDelivery.Entities;
using Microsoft.EntityFrameworkCore;

public class FoodContext : DbContext
{
    public DbSet<Shipper> Shippers { get; set; }
    public DbSet<Staff> Staff { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public FoodContext(DbContextOptions<FoodContext> options ) : base(options) {}
    public DbSet<Restaurant> Restaurants{get; set;} 
    public DbSet<Address> Addresses{get;set;}
    public DbSet<Cart> Carts{get;set;}
    public DbSet<CartItem> CartItems{get;set;}
    public DbSet<Category> Categories{get;set;}
    public DbSet<Order> Orders{get;set;}
    public DbSet<OrderDetail> OrderDetails{get;set;}
    public DbSet<OrderItem> OrderItems{get;set;}
    public DbSet<OrderStatusHistory> OrderStatusHistories{get;set;}
    public DbSet<Product> Products{get;set;}
    public DbSet<RefreshToken> RefreshTokens{get;set;}
    public DbSet<Review> Reviews{get;set;}
    public DbSet<Role> Roles{get;set;}
    public DbSet<User> Users{get;set;}
    public DbSet<UserRole> UserRoles{get;set;}
    public DbSet<PasswordResetOtp> PasswordResetOtp{get; set;}
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        // (OPTIONAL) logic custom
        return await base.SaveChangesAsync(cancellationToken);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Admin"},
            new Role { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "RestaurantOwner" },
            new Role { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "RestaurantStaff" },
            new Role { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "Shipper" },
            new Role { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), Name = "Customer" }
        );
        
        var permissions = typeof(PermissionConstants)
        .GetNestedTypes()
        .SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy))
        .Where(f => f.IsLiteral && !f.IsInitOnly)
        .Select(f => new Permission
        {
            Id = PermissionIdGenerator.Generate(f.GetRawConstantValue()?.ToString() ?? string.Empty),
            Name = f.GetRawConstantValue()?.ToString() ?? string.Empty,
            Description = $"Quyền cho {f.ReflectedType?.Name ?? "UnknownClass"} - {f.Name}",
            CreatedAt = DateTime.UtcNow
        })
        .ToList();

        modelBuilder.Entity<Permission>().HasData(permissions);
        modelBuilder.Entity<User>()
            .HasIndex(u=>u.Email)
            .IsUnique();
        modelBuilder.Entity<UserRole>()
            .HasKey(ur=>new {ur.UserId, ur.RoleId});
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);
        modelBuilder.Entity<UserRole>()
            .HasOne(ur=>ur.Role)
            .WithMany(r=>r.UserRoles)
            .HasForeignKey(ur=>ur.RoleId);
        modelBuilder.Entity<RefreshToken>()
            .HasOne(rf=>rf.User)
            .WithMany(u=>u.RefreshTokens)
            .HasForeignKey(rt=>rt.UserId);  
        modelBuilder.Entity<PasswordResetOtp>()
            .HasOne(pr=>pr.User)
            .WithMany(u=>u.PasswordResetOtps)
            .HasForeignKey(pr=>pr.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<PasswordResetOtp>()
            .HasIndex(x => x.UserId);
        modelBuilder.Entity<PasswordResetOtp>()
            .HasIndex(x => x.ExpiresAt);
        
        modelBuilder.Entity<User>()
            .HasOne(u=>u.Cart)
            .WithOne(c=>c.Customer)
            .HasForeignKey<Cart>(c=>c.CustomerId);

        modelBuilder.Entity<Review>()
            .HasOne(r=>r.Customer)
            .WithMany(u=>u.Reviews)
            .HasForeignKey(r=>r.CustomerId);
        modelBuilder.Entity<Order>()
            .HasIndex(o => o.OrderCode)
            .IsUnique();
        modelBuilder.Entity<OrderItem>()
            .HasOne(o=>o.Review)
            .WithOne(r=>r.OrderItem)
            .HasForeignKey<Review>(r=>r.OrderItemId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi=>oi.Order)
            .WithMany(o=>o.OrderItems)
            .HasForeignKey(oi=>oi.OrderId);

        modelBuilder.Entity<Order>()
            .HasOne(o=>o.Customer)
            .WithMany(u=>u.Orders)
            .HasForeignKey(o=>o.CustomerId);
        
        modelBuilder.Entity<Address>()
            .HasOne(a=>a.User)
            .WithMany(u=>u.Addresses)
            .HasForeignKey(a=>a.UserId);

        modelBuilder.Entity<CartItem>()
            .HasIndex(ci=>new {ci.CartId,ci.ProductId})
            .IsUnique(); // 1 loại product không đc xuất hiện trong 1 cart
        modelBuilder.Entity<CartItem>()
            .HasOne(ci=>ci.Cart)
            .WithMany(c=>c.CartItems)
            .HasForeignKey(ci=>ci.CartId);
        modelBuilder.Entity<CartItem>()
            .HasOne(ci=>ci.Product)
            .WithMany(p=>p.CartItems)
            .HasForeignKey(ci=>ci.ProductId);

        modelBuilder.Entity<Product>()
            .HasOne(p=>p.Category)
            .WithMany(c=>c.Products)
            .HasForeignKey(p=>p.CategoryId);

        modelBuilder.Entity<Review>()
            .HasOne(r=>r.Product)
            .WithMany(p=>p.Reviews)
            .HasForeignKey(r=>r.ProductId);
        modelBuilder.Entity<OrderStatusHistory>()
            .HasOne(osh=>osh.ChangeByUser)
            .WithMany(u=>u.OrderStatusHistories)
            .HasForeignKey(osh=>osh.ChangeByUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Category>(entity =>
        {
        entity.Property(e => e.CreatedAt)
              .HasColumnType("timestamptz")
              .HasDefaultValueSql("now()");

        entity.Property(e => e.UpdatedAt)
              .HasColumnType("timestamptz");
        });
        modelBuilder.Entity<Shipper>()
            .HasOne(s => s.User)
            .WithOne(u => u.Shipper)
            .HasForeignKey<Shipper>(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            // 1–1: Order ↔ OrderDetail
            entity.HasOne(od => od.Order)
                .WithOne(o => o.OrderDetail)
                .HasForeignKey<OrderDetail>(od => od.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
            // Shipper (OrderDetail → User)
            entity.HasOne(od => od.Shipper)
                .WithMany(s=>s.Orders) 
                .HasForeignKey(od => od.ShipperId)
                .OnDelete(DeleteBehavior.Restrict);
            //  Người hủy đơn (OrderDetail → User)
            entity.HasOne(od => od.CancelledByUser)
                .WithMany() 
                .HasForeignKey(od => od.CancelledByUserId)
                .OnDelete(DeleteBehavior.Restrict);
            // Enum → int
            entity.Property(od => od.Status)
                .HasConversion<int>();
            entity.Property(od => od.PaymentMethod)
                .HasConversion<int>();
            entity.Property(od => od.PaymentStatus)
                .HasConversion<int>();
            // Required / Optional
            entity.Property(od => od.CancelReason)
                .HasMaxLength(500);
            entity.Property(od => od.EstimatedDeliveryTime)
                .IsRequired(false);
            entity.Property(od => od.ActualDeliveryTime)
                .IsRequired(false);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(n => n.Type)
                .HasConversion<int>();

            entity.Property(n => n.Title)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(n => n.Message)
                .HasMaxLength(1000)
                .IsRequired();

            entity.Property(n => n.Link)
                .HasMaxLength(500);

            entity.HasIndex(n => new { n.UserId, n.CreatedAt })
                .IsDescending();
        });
        modelBuilder.Entity<Restaurant>()
            .HasMany(r => r.Products)
            .WithOne(p => p.Restaurant)
            .HasForeignKey(p => p.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Restaurant>()
            .HasMany(r => r.Categories)
            .WithOne(c => c.Restaurant)
            .HasForeignKey(c => c.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Restaurant>()
            .HasMany(r => r.Staff)
            .WithOne(s => s.Restaurant)
            .HasForeignKey(s => s.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Restaurant>()
            .HasMany(r => r.Orders)
            .WithOne(o => o.Restaurant)
            .HasForeignKey(o => o.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Restaurant>()
            .HasMany(r => r.Reviews)
            .WithOne(rv => rv.Restaurant)
            .HasForeignKey(rv => rv.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ProductImage>()
            .HasOne(pi => pi.Product)
            .WithMany(p=>p.ProductImages)
            .HasForeignKey(pi=>pi.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<RolePermission>()
            .HasKey(rp =>new {rp.RoleId, rp.PermissionId});
        
        //seed role-permission
        var rolePermissions = new Dictionary<Guid, List<string>>
        {
            [Guid.Parse("11111111-1111-1111-1111-111111111111")] = permissions.Select(p => p.Name).ToList(), // Admin có tất cả quyền
            [Guid.Parse("22222222-2222-2222-2222-222222222222")] = new List<string>
            {
                PermissionConstants.Products.View,
                PermissionConstants.Products.Create,
                PermissionConstants.Products.Edit,
                PermissionConstants.Products.Delete,
                PermissionConstants.Products.ToggleAvailability,

                PermissionConstants.Categories.View,
                PermissionConstants.Categories.Manage,

                PermissionConstants.Orders.View,
                PermissionConstants.Orders.Confirm,
                PermissionConstants.Orders.Cancel,
                PermissionConstants.Orders.UpdateStatus,
                PermissionConstants.Orders.AssignShipper,

                PermissionConstants.Restaurants.ViewProfile,
                PermissionConstants.Restaurants.UpdateProfile,
                PermissionConstants.Restaurants.ManageSettings,
                PermissionConstants.Restaurants.ViewRevenue,

                PermissionConstants.Staffs.View,
                PermissionConstants.Staffs.Create,
                PermissionConstants.Staffs.Edit,
                PermissionConstants.Staffs.Delete,

                PermissionConstants.Users.View,
                PermissionConstants.Users.ManageReviews
            },

            [Guid.Parse("33333333-3333-3333-3333-333333333333")] = new List<string>
            {
                PermissionConstants.Products.View,
                PermissionConstants.Products.ToggleAvailability,
                PermissionConstants.Categories.View,
                PermissionConstants.Orders.View,
                PermissionConstants.Orders.UpdateStatus,
                PermissionConstants.Restaurants.ViewProfile
            },

            [Guid.Parse("44444444-4444-4444-4444-444444444444")] = new List<string>
            {
                PermissionConstants.Orders.View,
                PermissionConstants.Orders.UpdateStatus,
                PermissionConstants.Shippers.AcceptOrder
            },

            [Guid.Parse("55555555-5555-5555-5555-555555555555")] = new List<string>
            {
                PermissionConstants.Products.View,
                PermissionConstants.Categories.View,
                PermissionConstants.Orders.Create,
                PermissionConstants.Orders.View
            }
        };
        var rolePermissionData = new List<RolePermission>();
        foreach (var role in rolePermissions)
        {
            foreach(var permissionName in role.Value)
            {
                rolePermissionData.Add(new RolePermission
                {
                    RoleId = role.Key,
                    PermissionId = PermissionIdGenerator.Generate(permissionName)
                });
            }
        }
        modelBuilder.Entity<RolePermission>().HasData(rolePermissionData);
    }

}