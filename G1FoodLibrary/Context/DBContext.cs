using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FoodLibrary.Context
{
    public partial class DBContext : DbContext
    {
        public DBContext()
        {
        }

        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Voucher> Vouchers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }

        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();
            var strConn = config["ConnectionString:G1FoodDB"];
            return strConn;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("ACCOUNT");

                entity.Property(e => e.AccountId)
                    .HasMaxLength(6)
                    .HasColumnName("AccountID");

                entity.Property(e => e.AccountAddress)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.AccountBirthDay).HasColumnType("date");

                entity.Property(e => e.AccountEmail)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.AccountName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.AccountPassword)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.AccountPhone)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.AccountStartDate).HasColumnType("date");

                entity.Property(e => e.AccountStatus)
                    .IsRequired()
                    .HasMaxLength(7);
            });

            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("CART");

                entity.Property(e => e.CartId).HasColumnName("CartID");

                entity.Property(e => e.AccountId)
                    .IsRequired()
                    .HasMaxLength(6)
                    .HasColumnName("AccountID");

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(6)
                    .HasColumnName("ProductID");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CART__AccountID__31EC6D26");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Carts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CART__ProductID__30F848ED");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("CATEGORY");

                entity.Property(e => e.CategoryId)
                    .HasMaxLength(5)
                    .HasColumnName("CategoryID");

                entity.Property(e => e.CategoryDescription)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("ORDER");

                entity.Property(e => e.OrderId)
                    .HasMaxLength(6)
                    .HasColumnName("OrderID");

                entity.Property(e => e.AccIdofChef)
                    .HasMaxLength(6)
                    .HasColumnName("AccIDOfChef");

                entity.Property(e => e.AccIdofShipper)
                    .HasMaxLength(6)
                    .HasColumnName("AccIDOfShipper");

                entity.Property(e => e.AccountId)
                    .IsRequired()
                    .HasMaxLength(6)
                    .HasColumnName("AccountID");

                entity.Property(e => e.BuyerAddress)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.BuyerFullName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.BuyerPhone)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.OrderNote)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.OrderStatus)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.VoucherId)
                    .HasMaxLength(20)
                    .HasColumnName("VoucherID");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ORDER__AccountID__34C8D9D1");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDid)
                    .HasName("PK__ORDER_DE__F4F410AAA582851E");

                entity.ToTable("ORDER_DETAIL");

                entity.Property(e => e.OrderDid)
                    .HasMaxLength(6)
                    .HasColumnName("OrderDID");

                entity.Property(e => e.OrderDprice).HasColumnName("OrderDPrice");

                entity.Property(e => e.OrderDquantity).HasColumnName("OrderDQuantity");

                entity.Property(e => e.OrderId)
                    .IsRequired()
                    .HasMaxLength(6)
                    .HasColumnName("OrderID");

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(6)
                    .HasColumnName("ProductID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ORDER_DET__Order__38996AB5");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ORDER_DET__Produ__37A5467C");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("PRODUCT");

                entity.Property(e => e.ProductId)
                    .HasMaxLength(6)
                    .HasColumnName("ProductID");

                entity.Property(e => e.CategoryId)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasColumnName("CategoryID");

                entity.Property(e => e.ProductDescription)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.ProductImage).IsRequired();

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ProductStatus)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PRODUCT__Categor__2A4B4B5E");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("REVIEW");

                entity.Property(e => e.ReviewId)
                    .HasMaxLength(6)
                    .HasColumnName("ReviewID");

                entity.Property(e => e.AccountId)
                    .IsRequired()
                    .HasMaxLength(6)
                    .HasColumnName("AccountID");

                entity.Property(e => e.ProductId)
                    .IsRequired()
                    .HasMaxLength(6)
                    .HasColumnName("ProductID");

                entity.Property(e => e.ReplyId)
                    .HasMaxLength(6)
                    .HasColumnName("ReplyID");

                entity.Property(e => e.Review1)
                    .HasMaxLength(500)
                    .HasColumnName("Review");

                entity.Property(e => e.ReviewDay).HasColumnType("datetime");

                entity.Property(e => e.ReviewStatus)
                    .IsRequired()
                    .HasMaxLength(7);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__REVIEW__AccountI__2E1BDC42");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__REVIEW__ProductI__2D27B809");
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.ToTable("VOUCHER");

                entity.Property(e => e.VoucherId)
                    .HasMaxLength(20)
                    .HasColumnName("VoucherID");

                entity.Property(e => e.VoucherDescription)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.VoucherEndday).HasColumnType("date");

                entity.Property(e => e.VoucherStartDay).HasColumnType("date");

                entity.Property(e => e.VoucherStatus)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
