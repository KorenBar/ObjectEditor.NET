using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;
using Microsoft.EntityFrameworkCore;
using ObjectEditor;

namespace DemoCommerceDbApp
{
    internal class CommerceDbContext : BaseDbContext
    {
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        public CommerceDbContext(string connectionString)
            : base(DbProvider.SQLite, connectionString)
        {
            if (Database.EnsureCreated())
                SeedData();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRole>().HasKey(ur => new { ur.UserId, ur.RoleId });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .UseLazyLoadingProxies()
                .EnableSensitiveDataLogging()
                .LogTo((s) => Debug.WriteLine(s));
        }

        private void SeedData()
        {
            // Add tenants
            var tenant1 = new Tenant { Id = Guid.NewGuid(), Name = "Tenant A", Domain = "tenanta.com", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            var tenant2 = new Tenant { Id = Guid.NewGuid(), Name = "Tenant B", Domain = "tenantb.com", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            Tenants.AddRange(tenant1, tenant2);
            SaveChanges();

            // Add roles
            var roleAdmin = new Role { Id = Guid.NewGuid(), Name = "Admin", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            var roleManager = new Role { Id = Guid.NewGuid(), Name = "Manager", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            var roleUser = new Role { Id = Guid.NewGuid(), Name = "User", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            Roles.AddRange(roleAdmin, roleManager, roleUser);
            SaveChanges();

            // Add users
            var user1 = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Alice",
                LastName = "Admin",
                Email = "alice@tenanta.com",
                Password = "adminpass",
                TenantId = tenant1.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var user1a = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Gary",
                LastName = "Manager",
                Email = "gary@tenanta.com",
                Password = "managerpass",
                TenantId = tenant1.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var user2 = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Bob",
                LastName = "User",
                Email = "bob@tenantb.com",
                Password = "userpass",
                TenantId = tenant2.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            Users.AddRange(user1, user1a, user2);
            SaveChanges();

            // Add user roles
            UserRoles.AddRange(
                new UserRole { UserId = user1.Id, RoleId = roleAdmin.Id, Permissions = Permissions.All }, // Alice can do anything as an admin
                new UserRole { UserId = user1.Id, RoleId = roleManager.Id, Permissions = Permissions.All }, // Alice can do anything as a manager

                new UserRole { UserId = user1a.Id, RoleId = roleAdmin.Id, Permissions = Permissions.Read }, // Gary can only read admin data (such as user passwords)
                new UserRole { UserId = user1a.Id, RoleId = roleManager.Id, Permissions = Permissions.All }, // Gary can manage all data

                new UserRole { UserId = user2.Id, RoleId = roleUser.Id, Permissions = Permissions.All } // admin and manager roles are not visible to Bob
            );
            SaveChanges();

            // Add categories
            var category1 = new Category { Id = Guid.NewGuid(), Name = "Electronics", TenantId = tenant1.Id, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            var category2 = new Category { Id = Guid.NewGuid(), Name = "Home Appliances", TenantId = tenant2.Id, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            Categories.AddRange(category1, category2);
            SaveChanges();

            // Add products
            var product1 = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Smartphone",
                Price = 699.99M,
                TenantId = tenant1.Id,
                CategoryId = category1.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var product2 = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Vacuum Cleaner",
                Price = 299.99M,
                TenantId = tenant2.Id,
                CategoryId = category2.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            Products.AddRange(product1, product2);
            SaveChanges();

            // Add orders
            var order1 = new Order
            {
                Id = Guid.NewGuid(),
                TenantId = tenant1.Id,
                UserId = user1.Id,
                TotalAmount = 699.99M,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var order2 = new Order
            {
                Id = Guid.NewGuid(),
                TenantId = tenant2.Id,
                UserId = user2.Id,
                TotalAmount = 299.99M,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            Orders.AddRange(order1, order2);
            SaveChanges();

            // Add order items
            OrderItems.AddRange(
                new OrderItem
                {
                    Id = Guid.NewGuid(),
                    OrderId = order1.Id,
                    ProductId = product1.Id,
                    Quantity = 1,
                    UnitPrice = 699.99M
                },
                new OrderItem
                {
                    Id = Guid.NewGuid(),
                    OrderId = order2.Id,
                    ProductId = product2.Id,
                    Quantity = 1,
                    UnitPrice = 299.99M
                }
            );
            SaveChanges();
        }
    }
}
