using System;
using System.Collections.Generic;
using CafeShop.Common;
using Microsoft.EntityFrameworkCore;

namespace CafeShop.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Cart { get; set; }
    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountImage> AccountImages { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductDetails> ProductDetails { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<ProductSize> ProductSizes { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(Config.Connection());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address).HasMaxLength(250);
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.FullName).HasMaxLength(250);
            entity.Property(e => e.PassWord).HasMaxLength(250);
            entity.Property(e => e.PhoneNumber).HasMaxLength(250);
            entity.Property(e => e.IsActive).HasColumnName("IsActive");
            entity.Property(e => e.Role).HasColumnName("Role");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable("Cart");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ProductDetailId).HasColumnName("ProductDetailId");
            entity.Property(e => e.AccountId).HasColumnName("AccountId");
            entity.Property(e => e.Quantity).HasColumnName("Quantity");
        });

        modelBuilder.Entity<AccountImage>(entity =>
        {
            entity.ToTable("AccountImage");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.ImageId2).HasColumnName("ImageID2");
        });

        
        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Address).HasMaxLength(250);
            entity.Property(e => e.OrderCode).HasMaxLength(250);    
            entity.Property(e => e.CreateBy).HasMaxLength(250);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.CustomerName).HasMaxLength(250);
            entity.Property(e => e.PhoneNumber).HasMaxLength(250);
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductDetailId).HasColumnName("ProductDetailID");
            entity.Property(e => e.TotalMoney).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code).HasMaxLength(250);
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.ProductTypeId).HasColumnName("ProductTypeID");
        });

        modelBuilder.Entity<ProductDetails>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductId).HasColumnName("ProductId");
            entity.Property(e => e.ProductSizeId).HasColumnName("ProductSizeID");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.ToTable("ProductImage");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ImageUrl).HasColumnName("ImageUrl");
            entity.Property(e => e.FileName).HasColumnName("FileName");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
        });

        modelBuilder.Entity<ProductSize>(entity =>
        {
            entity.ToTable("ProductSize");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code).HasMaxLength(250);
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.Name).HasMaxLength(250);
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.ToTable("ProductType");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code).HasMaxLength(250);
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.GroupType).HasColumnName("GroupType");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
