using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MuseMusic.Models.Tables
{
    public partial class shopmanagementContext : DbContext
    {
        public shopmanagementContext()
        {
        }

        public shopmanagementContext(DbContextOptions<shopmanagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Accessory> Accessories { get; set; } = null!;
        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<AccountRole> AccountRoles { get; set; } = null!;
        public virtual DbSet<Adminseller> Adminsellers { get; set; } = null!;
        public virtual DbSet<Artist> Artists { get; set; } = null!;
        public virtual DbSet<ArtistVinyl> ArtistVinyls { get; set; } = null!;
        public virtual DbSet<Blog> Blogs { get; set; } = null!;
        public virtual DbSet<Brand> Brands { get; set; } = null!;
        public virtual DbSet<Cart> Carts { get; set; } = null!;
        public virtual DbSet<CartDetail> CartDetails { get; set; } = null!;
        public virtual DbSet<CategoriesVinyl> CategoriesVinyls { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Mood> Moods { get; set; } = null!;
        public virtual DbSet<MoodVinyl> MoodVinyls { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Recordplayer> Recordplayers { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Vinyl> Vinyls { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;database=shopmanagement;user=root;password=123456;allow user variables=True", Microsoft.EntityFrameworkCore.ServerVersion.Parse("9.1.0-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Accessory>(entity =>
            {
                entity.ToTable("accessories");

                entity.HasIndex(e => e.BrandId, "brand_id");

                entity.HasIndex(e => e.ProductId, "product_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BrandId).HasColumnName("brand_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Accessories)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("accessories_ibfk_2");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Accessories)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("accessories_ibfk_1");
            });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("account");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<AccountRole>(entity =>
            {
                entity.ToTable("account_role");

                entity.HasIndex(e => e.AccountId, "account_id");

                entity.HasIndex(e => e.RoleId, "role_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.AccountRoles)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("account_role_ibfk_1");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AccountRoles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("account_role_ibfk_2");
            });

            modelBuilder.Entity<Adminseller>(entity =>
            {
                entity.ToTable("adminseller");

                entity.HasIndex(e => e.AccountId, "account_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .HasColumnName("address");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .HasColumnName("phone");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Adminsellers)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("adminseller_ibfk_1");
            });

            modelBuilder.Entity<Artist>(entity =>
            {
                entity.ToTable("artist");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<ArtistVinyl>(entity =>
            {
                entity.ToTable("artist_vinyl");

                entity.HasIndex(e => e.ArtistId, "artist_id");

                entity.HasIndex(e => e.VinylId, "vinyl_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ArtistId).HasColumnName("artist_id");

                entity.Property(e => e.VinylId).HasColumnName("vinyl_id");

                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.ArtistVinyls)
                    .HasForeignKey(d => d.ArtistId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("artist_vinyl_ibfk_1");

                entity.HasOne(d => d.Vinyl)
                    .WithMany(p => p.ArtistVinyls)
                    .HasForeignKey(d => d.VinylId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("artist_vinyl_ibfk_2");
            });

            modelBuilder.Entity<Blog>(entity =>
            {
                entity.ToTable("blog");

                entity.HasIndex(e => e.AdminId, "admin_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AdminId).HasColumnName("admin_id");

                entity.Property(e => e.CreateAt)
                    .HasColumnType("timestamp")
                    .HasColumnName("Create_at");

                entity.Property(e => e.DescriBlog).HasMaxLength(255);

                entity.Property(e => e.Nameblog)
                    .HasMaxLength(255)
                    .HasColumnName("nameblog");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.Blogs)
                    .HasForeignKey(d => d.AdminId)
                    .HasConstraintName("blog_ibfk_1");
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("brand");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Website)
                    .HasMaxLength(255)
                    .HasColumnName("website");
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("cart");

                entity.HasIndex(e => e.CustomerId, "customer_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("cart_ibfk_1");
            });

            modelBuilder.Entity<CartDetail>(entity =>
            {
                entity.ToTable("cart_detail");

                entity.HasIndex(e => e.CartId, "cart_id");

                entity.HasIndex(e => e.ProductId, "product_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CartId).HasColumnName("cart_id");

                entity.Property(e => e.Price)
                    .HasPrecision(10, 2)
                    .HasColumnName("price");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Cart)
                    .WithMany(p => p.CartDetails)
                    .HasForeignKey(d => d.CartId)
                    .HasConstraintName("cart_detail_ibfk_1");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CartDetails)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("cart_detail_ibfk_2");
            });

            modelBuilder.Entity<CategoriesVinyl>(entity =>
            {
                entity.ToTable("categories_vinyl");

                entity.HasIndex(e => e.CategoryId, "category_id");

                entity.HasIndex(e => e.VinylId, "vinyl_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.VinylId).HasColumnName("vinyl_id");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.CategoriesVinyls)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("categories_vinyl_ibfk_2");

                entity.HasOne(d => d.Vinyl)
                    .WithMany(p => p.CategoriesVinyls)
                    .HasForeignKey(d => d.VinylId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("categories_vinyl_ibfk_1");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("category");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customer");

                entity.HasIndex(e => e.AccountId, "account_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .HasColumnName("address");

                entity.Property(e => e.Image)
                    .HasMaxLength(255)
                    .HasColumnName("image");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .HasColumnName("phone");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("customer_ibfk_1");
            });

            modelBuilder.Entity<Mood>(entity =>
            {
                entity.ToTable("mood");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<MoodVinyl>(entity =>
            {
                entity.ToTable("mood_vinyl");

                entity.HasIndex(e => e.MoodId, "mood_id");

                entity.HasIndex(e => e.VinylId, "vinyl_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.MoodId).HasColumnName("mood_id");

                entity.Property(e => e.VinylId).HasColumnName("vinyl_id");

                entity.HasOne(d => d.Mood)
                    .WithMany(p => p.MoodVinyls)
                    .HasForeignKey(d => d.MoodId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("mood_vinyl_ibfk_1");

                entity.HasOne(d => d.Vinyl)
                    .WithMany(p => p.MoodVinyls)
                    .HasForeignKey(d => d.VinylId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("mood_vinyl_ibfk_2");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");

                entity.HasIndex(e => e.AdminsellerId, "adminseller_id");

                entity.HasIndex(e => e.CustomerId, "customer_id");

                entity.HasIndex(e => e.PaymentId, "payment_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AdminsellerId).HasColumnName("adminseller_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.DeliveredAt)
                    .HasColumnType("datetime")
                    .HasColumnName("delivered_at");

                entity.Property(e => e.ExpectedEndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("expected_end_date");

                entity.Property(e => e.ExpectedStartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("expected_start_date");

                entity.Property(e => e.PaymentId).HasColumnName("payment_id");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");

                entity.Property(e => e.Total)
                    .HasPrecision(10, 2)
                    .HasColumnName("total");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("updated_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.Adminseller)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.AdminsellerId)
                    .HasConstraintName("orders_ibfk_2");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("orders_ibfk_1");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.PaymentId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("orders_ibfk_3");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("order_detail");

                entity.HasIndex(e => e.OrderId, "order_id");

                entity.HasIndex(e => e.ProductId, "product_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.OrderId).HasColumnName("order_id");

                entity.Property(e => e.Price)
                    .HasPrecision(10, 2)
                    .HasColumnName("price");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("order_detail_ibfk_1");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("order_detail_ibfk_2");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("payment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Method)
                    .HasMaxLength(50)
                    .HasColumnName("method");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("product");

                entity.HasIndex(e => e.AdminsellerId, "adminseller_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AdminsellerId).HasColumnName("adminseller_id");

                entity.Property(e => e.Description)
                    .HasColumnType("text")
                    .HasColumnName("description");

                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(255)
                    .HasColumnName("image_url");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Price)
                    .HasPrecision(10, 2)
                    .HasColumnName("price");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Adminseller)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.AdminsellerId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("product_ibfk_1");
            });

            modelBuilder.Entity<Recordplayer>(entity =>
            {
                entity.ToTable("recordplayer");

                entity.HasIndex(e => e.BrandId, "brand_id");

                entity.HasIndex(e => e.ProductId, "product_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BrandId).HasColumnName("brand_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Recordplayers)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("recordplayer_ibfk_2");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Recordplayers)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("recordplayer_ibfk_1");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Vinyl>(entity =>
            {
                entity.ToTable("vinyl");

                entity.HasIndex(e => e.BrandId, "brand_id");

                entity.HasIndex(e => e.ProductId, "product_id");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BrandId).HasColumnName("brand_id");

                entity.Property(e => e.DiskId)
                    .HasMaxLength(255)
                    .HasColumnName("disk_id");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Tracklist)
                    .HasColumnType("text")
                    .HasColumnName("tracklist");

                entity.Property(e => e.Years).HasColumnName("years");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Vinyls)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("vinyl_ibfk_2");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Vinyls)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("vinyl_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
