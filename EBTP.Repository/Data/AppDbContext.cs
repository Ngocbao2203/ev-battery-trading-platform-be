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
        public DbSet<Package> Package { get; set; }
        public DbSet<Report> Report { get; set; }
        public DbSet<Favourite> Favourite { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<ChatThread> ChatThread { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
   new Role { Id = 1, RoleName = "Admin" },
   new Role { Id = 2, RoleName = "User" },
   new Role { Id = 3, RoleName = "Staff" }
);
            modelBuilder.Entity<User>().HasData(
     new User
     {
         Id = Guid.Parse("8B56687E-8377-4743-AAC9-08DCF5C4B471"),
         UserName = "Admin",
         Email = "admin@gmail.com",
         PasswordHash = "$2y$10$O1smXu1TdT1x.Z35v5jQauKcQIBn85VYRqiLggPD8HMF9rRyGnHXy",
         Status = StatusEnum.Active,
         RoleId = 1,
         IsVerified = true,
         PhoneNumber = "0123456789",
         CreationDate = new DateTime(2024, 01, 01, 00, 00, 00, DateTimeKind.Utc),
         IsDeleted = false
     },
     new User
     {
         Id = Guid.Parse("8B56687E-8377-4743-AAC9-08DCF5C4B47F"),
         UserName = "Staff",
         Email = "staff@gmail.com",
         PasswordHash = "$2y$10$O1smXu1TdT1x.Z35v5jQauKcQIBn85VYRqiLggPD8HMF9rRyGnHXy",
         Status = StatusEnum.Active,
         RoleId = 3,
         IsVerified = true,
         PhoneNumber = "0123456789",
         CreationDate = new DateTime(2024, 01, 01, 00, 00, 00, DateTimeKind.Utc),
         IsDeleted = false
     }
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

            modelBuilder.Entity<Listing>()
            .HasOne(p => p.User)
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
            //Package
            modelBuilder.Entity<Package>(e =>
            {
                e.ToTable("Package");
                e.HasKey(p => p.Id);
                e.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);
                e.Property(p => p.Description)
                .HasMaxLength(500);
                e.Property(p => p.Status)
                .IsRequired()
                .HasConversion(v => v.ToString(), v => (StatusEnum)System.Enum.Parse(typeof(StatusEnum), v));
                e.Property(p => p.PackageType)
                .IsRequired()
                .HasConversion(v => v.ToString(), v => (PackageTypeEnum)System.Enum.Parse(typeof(PackageTypeEnum), v));
            });
        }
    }
}
