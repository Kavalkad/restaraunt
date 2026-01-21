using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using restaraunt.Core.Entities;

namespace restaraunt.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id)
                .ValueGeneratedOnAdd();

            builder.Property(o => o.TableNumber)
                .IsRequired();

            builder.Property(o => o.CustomerName)
                .HasMaxLength(100);

            builder.Property(o => o.CreatedAt)
                .IsRequired();

            builder.Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(o => o.Notes)
                .HasMaxLength(500);

            builder
                .HasMany(o => o.Items)
                .WithOne(oi => oi.Order);
                
        }
    }
}