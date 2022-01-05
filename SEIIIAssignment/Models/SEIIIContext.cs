using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace SEIIIAssignment.Models
{
    public partial class SEIIIContext : DbContext
    {
        public SEIIIContext()
        {
        }

        public SEIIIContext(DbContextOptions<SEIIIContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Auction> Auctions { get; set; }
        public virtual DbSet<Bid> Bids { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Classification> Classifications { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=DevConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Auction>(entity =>
            {
                entity.ToTable("Auction");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Boughtby)
                    .WithMany(p => p.AuctionBoughtbies)
                    .HasForeignKey(d => d.BoughtbyId)
                    .HasConstraintName("FK_Auction_User");

                entity.HasOne(d => d.Postedby)
                    .WithMany(p => p.AuctionPostedbies)
                    .HasForeignKey(d => d.PostedbyId)
                    .HasConstraintName("FK_Auction_User1");
            });

            modelBuilder.Entity<Bid>(entity =>
            {
                entity.ToTable("Bid");

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Auction)
                    .WithMany(p => p.Bids)
                    .HasForeignKey(d => d.AuctionId)
                    .HasConstraintName("FK_Bid_Auction");

                entity.HasOne(d => d.Bidder)
                    .WithMany(p => p.Bids)
                    .HasForeignKey(d => d.BidderId)
                    .HasConstraintName("FK_Bid_User");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Classification>(entity =>
            {
                entity.ToTable("Classification");

                entity.Property(e => e.ClassificationName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("Item");

                entity.Property(e => e.Artist)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Image)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ItemType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Material)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Medium)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("medium");

                entity.Property(e => e.ProductName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TextualDescription).HasColumnType("text");

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Item_Category");

                entity.HasOne(d => d.Classification)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.ClassificationId)
                    .HasConstraintName("FK_Item_Classification");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
