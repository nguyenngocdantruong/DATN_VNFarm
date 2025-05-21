using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using VNFarm.Entities;

namespace VNFarm.Data
{
    public class VNFarmContext : DbContext
    {
        public VNFarmContext(DbContextOptions<VNFarmContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        // public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderTimeline> OrderTimelines { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        // public DbSet<PaymentMethod> PaymentMethods { get; set; }
        // public DbSet<BusinessRegistration> BusinessRegistrations { get; set; }
        // public DbSet<Transaction> Transactions { get; set; }
        // public DbSet<RegistrationApprovalResult> RegistrationApprovalResults { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<ShopCart> ShopCarts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        // public DbSet<ContactRequest> ContactRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and constraints
            
            // User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Store
            modelBuilder.Entity<User>()
                .HasOne(u => u.Store)
                .WithOne(s => s.User)
                .HasForeignKey<Store>(s => s.UserId);

            // Product
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Store)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.StoreId);

            // Product-Category
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .IsRequired(false);

            // Order
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Buyer)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.BuyerId);

            // modelBuilder.Entity<Order>()
            //     .HasOne(o => o.Store)
            //     .WithMany()
            //     .HasForeignKey(o => o.StoreId)
            //     .IsRequired(false);

            // modelBuilder.Entity<OrderDetail>()
            //     .HasOne(od => od.Product)
            //     .WithMany()
            //     .HasForeignKey(od => od.ProductId);
                
            // OrderItem
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);
                
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);
                
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Shop)
                .WithMany()
                .HasForeignKey(oi => oi.ShopId);

            // OrderTimeline
            modelBuilder.Entity<OrderTimeline>()
                .HasOne<Order>()
                .WithMany(o => o.OrderTimelines)
                .HasForeignKey(ot => ot.OrderId);

            // Review-User
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId);

            // Chat
            modelBuilder.Entity<Chat>()
                .HasOne(c => c.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(c => c.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Chat>()
                .HasOne(c => c.ChatRoom)
                .WithMany(cr => cr.Messages)
                .HasForeignKey(c => c.ChatRoomId);
                
            // ChatRoom relationships
            modelBuilder.Entity<ChatRoom>()
                .HasOne(cr => cr.Buyer)
                .WithMany()
                .HasForeignKey(cr => cr.BuyerId);
                
            modelBuilder.Entity<ChatRoom>()
                .HasOne(cr => cr.Seller)
                .WithMany()
                .HasForeignKey(cr => cr.SellerId);
                
            modelBuilder.Entity<ChatRoom>()
                .HasOne(cr => cr.Order)
                .WithMany()
                .HasForeignKey(cr => cr.OrderId)
                .IsRequired(false);

            // Discount
            modelBuilder.Entity<Discount>()
                .HasOne(d => d.Store)
                .WithMany(s => s.Discounts)
                .HasForeignKey(d => d.StoreId)
                .IsRequired(false);

            // Review-Product relationship
            modelBuilder.Entity<Review>()
                .HasOne<Product>()
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId)
                .IsRequired(false);

            // Order-Discount relationship
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Discount)
                .WithMany()
                .HasForeignKey(o => o.DiscountId)
                .IsRequired(false);

            // Transaction relationships
            // modelBuilder.Entity<Transaction>()
            //     .HasOne(t => t.Order)
            //     .WithMany()
            //     .HasForeignKey(t => t.OrderId)
            //     .IsRequired(false);
                
            // modelBuilder.Entity<Transaction>()
            //     .HasOne(t => t.Buyer)
            //     .WithMany()
            //     .HasForeignKey(t => t.BuyerId);

            // Notification-User relationship
            modelBuilder.Entity<Notification>()
                .HasOne<User>()
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId);

            // BusinessRegistration-User relationship
            // modelBuilder.Entity<BusinessRegistration>()
            //     .HasOne(br => br.User)
            //     .WithOne()
            //     .HasForeignKey<BusinessRegistration>(br => br.UserId);

            // PaymentMethod-User relationship
            // modelBuilder.Entity<PaymentMethod>()
            //     .HasOne<User>()
            //     .WithMany(u => u.PaymentMethods)
            //     .HasForeignKey(pm => pm.UserId);

            // RegistrationApprovalResult relationships
            // modelBuilder.Entity<RegistrationApprovalResult>()
            //     .HasOne<BusinessRegistration>()
            //     .WithMany(br => br.ApprovalResults)
            //     .HasForeignKey(rar => rar.RegistrationId);
                
            // modelBuilder.Entity<RegistrationApprovalResult>()
            //     .HasOne(rar => rar.Admin)
            //     .WithMany()
            //     .HasForeignKey(rar => rar.AdminId);

            // Cart-User relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.UserId);
            
            // ShopCart relationships
            modelBuilder.Entity<ShopCart>()
                .HasOne(sc => sc.Cart)
                .WithMany(c => c.ShopCarts)
                .HasForeignKey(sc => sc.CartId);
            
            modelBuilder.Entity<ShopCart>()
                .HasOne(sc => sc.Shop)
                .WithMany()
                .HasForeignKey(sc => sc.ShopId);
            
            // CartItem relationships
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId);
            
            modelBuilder.Entity<CartItem>()
                .HasOne<ShopCart>()
                .WithMany(sc => sc.CartItems)
                .HasForeignKey(ci => ci.ShopCartId);

            // Apply global query filter for soft delete
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var property = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                    var falseValue = Expression.Constant(false);
                    var condition = Expression.Equal(property, falseValue);
                    var lambda = Expression.Lambda(condition, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        private void UpdateAuditFields()
        {
            var now = DateTime.UtcNow;
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.IsDeleted = false;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = now;
                    entry.Property(x => x.CreatedAt).IsModified = false;
                }
            }
        }
    }
} 