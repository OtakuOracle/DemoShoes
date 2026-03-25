using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DemoTest.Models;

public partial class TrenirovkaContext : DbContext
{
    public TrenirovkaContext()
    {
    }

    public TrenirovkaContext(DbContextOptions<TrenirovkaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<PickUpPoint> PickUpPoints { get; set; }

    public virtual DbSet<ProductInOrder> ProductInOrders { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Tovar> Tovars { get; set; }

    public virtual DbSet<TovarType> TovarTypes { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5438;Database=trenirovka;Username=nastya;Password=123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("category_pkey");

            entity.ToTable("category", "shoes");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasColumnType("character varying")
                .HasColumnName("category_name");
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.ManufacturerId).HasName("manufacturer_pkey");

            entity.ToTable("manufacturer", "shoes");

            entity.Property(e => e.ManufacturerId).HasColumnName("manufacturer_id");
            entity.Property(e => e.ManufacturerName)
                .HasColumnType("character varying")
                .HasColumnName("manufacturer_name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("order_pkey");

            entity.ToTable("order", "shoes");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.DateCreate).HasColumnName("date_create");
            entity.Property(e => e.DateDelivery).HasColumnName("date_delivery");
            entity.Property(e => e.OrderStatus).HasColumnName("order_status");
            entity.Property(e => e.PickUpPointId).HasColumnName("pick_up_point_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.OrderStatusNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderStatus)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("order_order_status_fkey");

            entity.HasOne(d => d.PickUpPoint).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PickUpPointId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("order_pick_up_point_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("order_user_id_fkey");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.OrderStatusId).HasName("order_status_pkey");

            entity.ToTable("order_status", "shoes");

            entity.Property(e => e.OrderStatusId).HasColumnName("order_status_id");
            entity.Property(e => e.OrderStatusName)
                .HasColumnType("character varying")
                .HasColumnName("order_status_name");
        });

        modelBuilder.Entity<PickUpPoint>(entity =>
        {
            entity.HasKey(e => e.PickUpPointId).HasName("pick_up_point_pkey");

            entity.ToTable("pick_up_point", "shoes");

            entity.Property(e => e.PickUpPointId).HasColumnName("pick_up_point_id");
            entity.Property(e => e.PickUpPointName)
                .HasColumnType("character varying")
                .HasColumnName("pick_up_point_name");
        });

        modelBuilder.Entity<ProductInOrder>(entity =>
        {
            entity.HasKey(e => e.ProductInOrderId).HasName("product_in_order_pkey");

            entity.ToTable("product_in_order", "shoes");

            entity.Property(e => e.ProductInOrderId).HasColumnName("product_in_order_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.TovarArt)
                .HasColumnType("character varying")
                .HasColumnName("tovar_art");

            entity.HasOne(d => d.Order).WithMany(p => p.ProductInOrders)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("product_in_order_order_id_fkey");

            entity.HasOne(d => d.TovarArtNavigation).WithMany(p => p.ProductInOrders)
                .HasForeignKey(d => d.TovarArt)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("product_in_order_tovar_art_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("role_pkey");

            entity.ToTable("role", "shoes");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasColumnType("character varying")
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("supplier_pkey");

            entity.ToTable("supplier", "shoes");

            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.SupplierName)
                .HasColumnType("character varying")
                .HasColumnName("supplier_name");
        });

        modelBuilder.Entity<Tovar>(entity =>
        {
            entity.HasKey(e => e.TovarArt).HasName("tovar_pkey");

            entity.ToTable("tovar", "shoes");

            entity.Property(e => e.TovarArt)
                .HasColumnType("character varying")
                .HasColumnName("tovar_art");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.Discount).HasColumnName("discount");
            entity.Property(e => e.ManufacturerId).HasColumnName("manufacturer_id");
            entity.Property(e => e.Photo)
                .HasColumnType("character varying")
                .HasColumnName("photo");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.SupplierId).HasColumnName("supplier_id");
            entity.Property(e => e.TovarTypeId).HasColumnName("tovar_type_id");
            entity.Property(e => e.UnitId).HasColumnName("unit_id");

            entity.HasOne(d => d.Category).WithMany(p => p.Tovars)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("tovar_category_id_fkey");

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.Tovars)
                .HasForeignKey(d => d.ManufacturerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("tovar_manufacturer_id_fkey");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Tovars)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("tovar_supplier_id_fkey");

            entity.HasOne(d => d.TovarType).WithMany(p => p.Tovars)
                .HasForeignKey(d => d.TovarTypeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("tovar_tovar_type_id_fkey");

            entity.HasOne(d => d.Unit).WithMany(p => p.Tovars)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("tovar_unit_id_fkey");
        });

        modelBuilder.Entity<TovarType>(entity =>
        {
            entity.HasKey(e => e.TovarTypeId).HasName("tovar_type_pkey");

            entity.ToTable("tovar_type", "shoes");

            entity.Property(e => e.TovarTypeId).HasColumnName("tovar_type_id");
            entity.Property(e => e.TovarTypeName)
                .HasColumnType("character varying")
                .HasColumnName("tovar_type_name");
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.HasKey(e => e.UnitId).HasName("unit_pkey");

            entity.ToTable("unit", "shoes");

            entity.Property(e => e.UnitId).HasColumnName("unit_id");
            entity.Property(e => e.UnitName)
                .HasColumnType("character varying")
                .HasColumnName("unit_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("user_pkey");

            entity.ToTable("user", "shoes");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Login)
                .HasColumnType("character varying")
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasColumnType("character varying")
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.UserName)
                .HasColumnType("character varying")
                .HasColumnName("user_name");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("user_role_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
