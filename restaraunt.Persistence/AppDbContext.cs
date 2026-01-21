using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using restaraunt.Persistence.Configurations;
using restaraunt.Core.Entities;


namespace restaraunt.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderItemEntity> OrderItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Users.db");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}