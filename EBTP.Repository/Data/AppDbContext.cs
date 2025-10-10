using EBTP.Repository.Entities;
using EBTP.Repository.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Repository.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        #region
        public DbSet<User> User { get; set; }
        public DbSet<Brand> Brand { get; set; }
        public DbSet<Listing> Listing { get; set; }
        public DbSet<ListingImage> ListingImage { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
               new Role { Id = 1, RoleName = "Admin" },
               new Role { Id = 2, RoleName = "User" }
            );
            //Brand
            modelBuilder.Entity<Brand>(e =>
            {
                e.ToTable("Brand");
                e.HasKey(p => p.Id);
                e.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);
            });

            //Listing
            modelBuilder.Entity<Listing>(e =>
            {
                e.ToTable("Listing");
                e.HasKey(p => p.Id);
                e.Property(p => p.Status)
                .IsRequired()
                .HasConversion(v => v.ToString(), v => (StatusEnum)System.Enum.Parse(typeof(StatusEnum), v));
                e.Property(p => p.Category)
                .IsRequired()
                .HasConversion(v => v.ToString(), v => (CategoryEnum)System.Enum.Parse(typeof(CategoryEnum), v));
                e.Property(p => p.ListingStatus)
                .IsRequired()
                .HasConversion(v => v.ToString(), v => (ListingStatusEnum)System.Enum.Parse(typeof(ListingStatusEnum), v));
            });

            modelBuilder.Entity<Listing>()
            .HasOne(p => p.Brand)
            .WithMany(p => p.Listings);

            //Listing Image
            modelBuilder.Entity<ListingImage>(e =>
            {
                e.ToTable("ListingImage");
                e.HasKey(p => p.Id);
                e.Property(p => p.ImageUrl)
                .IsRequired()
                .HasMaxLength(250);
                e.HasOne(p => p.Listing)
                .WithMany(p => p.ListingImages)
                .HasForeignKey(p => p.ListingId)
                .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
