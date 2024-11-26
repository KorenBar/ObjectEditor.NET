using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectEditor;

namespace DemoCommerceDbApp
{
    // Base class for audit and common properties
    public abstract class BaseEntity
    {
        [Key]
        [EditorIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();

        [NotMapped]
        public Guid _Id => Id; // readonly for the editor 

        [PermissionGroup("Admin")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [PermissionGroup("Admin")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class Tenant : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        [PermissionGroup("Admin")]
        [EditorDisplayName]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        [PermissionGroup("Admin")]
        public string Domain { get; set; }

        // Navigation properties
        //[EditorIgnore]
        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
        //[EditorIgnore]
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
        //[EditorIgnore]
        public virtual ICollection<Category> Categories { get; set; } = new HashSet<Category>();
        //[EditorIgnore]
        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();

        //// Dictionary wrapper for the editor
        //[NotMapped]
        //[PermissionGroup("Manager")]
        //public DictionaryWrapper<Guid, User> UsersDictionary => new DictionaryWrapper<Guid, User>(Users, u => u.Id, (u, id) => u.Id = id);
        //[NotMapped]
        //public DictionaryWrapper<Guid, Product> ProductsDictionary => new DictionaryWrapper<Guid, Product>(Products, p => p.Id, (p, id) => p.Id = id);
        //[NotMapped]
        //public DictionaryWrapper<Guid, Category> CategoriesDictionary => new DictionaryWrapper<Guid, Category>(Categories, c => c.Id, (c, id) => c.Id = id);
        //[NotMapped]
        //public DictionaryWrapper<Guid, Order> OrdersDictionary => new DictionaryWrapper<Guid, Order>(Orders, o => o.Id, (o, id) => o.Id = id);
    }

    public class User : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(150)]
        [EditorDisplayName]
        public string Email { get; set; }

        [Required]
        [PermissionGroup("Admin")]
        public string Password { get; set; } // should be hashed, but for demo purposes it's plain text

        [ForeignKey("Tenant")]
        public Guid TenantId { get; set; }
        [EditorIgnore]
        public virtual Tenant Tenant { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
    }

    public class Role : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        [EditorDisplayName]
        public string Name { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
    }

    public class UserRole
    {
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Role")]
        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }

        public Permissions Permissions { get; set; }
    }

    public class Product : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        [PermissionGroup("Manager")]
        [EditorDisplayName]
        public string Name { get; set; }

        [PermissionGroup("Manager")]
        public decimal Price { get; set; }

        [ForeignKey("Tenant")]
        [EditorIgnore]
        public Guid TenantId { get; set; }
        [EditorIgnore]
        public virtual Tenant Tenant { get; set; }

        [ForeignKey("Category")]
        [EditorIgnore]
        public Guid CategoryId { get; set; }
        [EditorIgnore]
        public virtual Category Category { get; set; }

        [EditorIgnore]
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
    }

    public class Category : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        [PermissionGroup("Manager")]
        [EditorDisplayName]
        public string Name { get; set; }

        [ForeignKey("Tenant")]
        [EditorIgnore]
        public Guid TenantId { get; set; }
        [EditorIgnore]
        public virtual Tenant Tenant { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }

    public class Order : BaseEntity
    {
        [ForeignKey("Tenant")]
        [EditorIgnore]
        public Guid TenantId { get; set; }
        [EditorIgnore]
        public virtual Tenant Tenant { get; set; }

        [ForeignKey("User")]
        [EditorIgnore]
        public Guid UserId { get; set; }
        [EditorIgnore]
        public virtual User User { get; set; }

        [NotMapped]
        [EditorIgnore]
        [EditorDisplayName]
        public string UserEmail => User?.Email;

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();

        [PermissionGroup("Manager")]
        public decimal TotalAmount { get; set; }
    }

    public class OrderItem
    {
        [Key]
        [EditorIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();

        [ForeignKey("Order")]
        public Guid OrderId { get; set; }
        [EditorIgnore]
        public virtual Order Order { get; set; }

        [ForeignKey("Product")]
        public Guid ProductId { get; set; }
        [EditorIgnore]
        public virtual Product Product { get; set; }

        [EditorDisplayName]
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        [NotMapped]
        [EditorIgnore]
        [EditorDisplayName]
        public string ProductName => Product?.Name;
    }

    public class AuditLog : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string ActionType { get; set; }

        [Required]
        [MaxLength(200)]
        public string EntityName { get; set; }

        public Guid EntityId { get; set; }
        public string Changes { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        [EditorIgnore]
        public virtual User User { get; set; }
    }
}
