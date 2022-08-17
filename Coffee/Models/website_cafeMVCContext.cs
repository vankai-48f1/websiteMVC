using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Coffee.Models
{
    public partial class website_cafeMVCContext : DbContext
    {
        public website_cafeMVCContext()
        {
        }

        public website_cafeMVCContext(DbContextOptions<website_cafeMVCContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Bill> Bills { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<DetailBill> DetailBills { get; set; }
        public virtual DbSet<DetailsPayment> DetailsPayments { get; set; }
        public virtual DbSet<Medium> Media { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<Unit> Units { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=VAGABONDCODER\\TUAN;Initial Catalog=website_cafeMVC;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("account");

                entity.HasIndex(e => e.Username, "Unique_Account")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.MediaId).HasColumnName("media_id");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Role)
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .HasColumnName("role")
                    .HasDefaultValueSql("('Customer')");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasDefaultValueSql("('1')");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__account__custome__2F10007B");

                entity.HasOne(d => d.Media)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.MediaId)
                    .HasConstraintName("FK__account__media_i__300424B4");
            });

            modelBuilder.Entity<Bill>(entity =>
            {
                entity.ToTable("bill");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasColumnName("created");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.SessionId).HasColumnName("session_id");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Total)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("total");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Bills)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__bill__customer_i__412EB0B6");

                entity.HasOne(d => d.Session)
                    .WithMany(p => p.Bills)
                    .HasForeignKey(d => d.SessionId)
                    .HasConstraintName("FK__bill__session_id__403A8C7D");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customer");

                entity.HasIndex(e => e.Phone, "UQ__customer__B43B145FF3DB9AD9")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasMaxLength(1000)
                    .HasColumnName("address");

                entity.Property(e => e.Birth)
                    .HasColumnType("date")
                    .HasColumnName("birth");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("full_name");

                entity.Property(e => e.Gender)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("gender");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.Point)
                    .HasColumnName("point")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<DetailBill>(entity =>
            {
                entity.ToTable("detail_bill");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BillId).HasColumnName("bill_id");

                entity.Property(e => e.Discount)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("discount");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.Size)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("size")
                    .HasDefaultValueSql("('S')")
                    .IsFixedLength();

                entity.Property(e => e.SubTotal)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("sub_total");

                entity.Property(e => e.UnitPrice)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("unit_price");

                entity.HasOne(d => d.Bill)
                    .WithMany(p => p.DetailBills)
                    .HasForeignKey(d => d.BillId)
                    .HasConstraintName("FK__detail_bi__bill___59FA5E80");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.DetailBills)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__detail_bi__produ__59063A47");
            });

            modelBuilder.Entity<DetailsPayment>(entity =>
            {
                entity.ToTable("detailsPayment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BillId).HasColumnName("bill_id");

                entity.Property(e => e.PaymentAmount)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("payment_amount");

                entity.Property(e => e.PaymentId).HasColumnName("payment_id");

                entity.HasOne(d => d.Bill)
                    .WithMany(p => p.DetailsPayments)
                    .HasForeignKey(d => d.BillId)
                    .HasConstraintName("FK__detailsPa__bill___4F7CD00D");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.DetailsPayments)
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("FK__detailsPa__payme__5070F446");
            });

            modelBuilder.Entity<Medium>(entity =>
            {
                entity.ToTable("media");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("file_name");

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("path");

                entity.Property(e => e.TimeCreate)
                    .HasColumnType("date")
                    .HasColumnName("time_create");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("payment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PaymentName)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasColumnName("payment_name");

                entity.Property(e => e.PaymentType).HasColumnName("payment_type");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("post");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.Category)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("category")
                    .HasDefaultValueSql("('Blog')");

                entity.Property(e => e.Content)
                    .HasMaxLength(1000)
                    .HasColumnName("content");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasColumnName("created");

                entity.Property(e => e.ShortDescription)
                    .HasMaxLength(1000)
                    .HasColumnName("short_description");

                entity.Property(e => e.Slug)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("slug");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Tag)
                    .HasMaxLength(1000)
                    .HasColumnName("tag");

                entity.Property(e => e.Title)
                    .HasMaxLength(1000)
                    .HasColumnName("title");

                entity.Property(e => e.Updated)
                    .HasColumnType("datetime")
                    .HasColumnName("updated");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK__post__account_id__35BCFE0A");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("product");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.MediaId).HasColumnName("media_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("price");

                entity.Property(e => e.ProductCategoryId).HasColumnName("product_category_id");

                entity.Property(e => e.UnitId).HasColumnName("unit_id");

                entity.HasOne(d => d.Media)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.MediaId)
                    .HasConstraintName("FK__product__media_i__49C3F6B7");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .HasConstraintName("FK__product__product__787EE5A0");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("FK__product__unit_id__48CFD27E");
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.ToTable("product_category");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.ToTable("session");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.CashTotal)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("cash_total");

                entity.Property(e => e.Closed)
                    .HasColumnType("datetime")
                    .HasColumnName("closed");

                entity.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasColumnName("created");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Total)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("total");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Sessions)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK__session__account__3B75D760");
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.ToTable("unit");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
