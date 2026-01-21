using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using restaraunt.Core.Entities;

namespace restaraunt.Persistence.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItemEntity>
    {
        public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
        {
            builder.HasKey(oi => oi.Id);
            builder.Property(oi => oi.Id)
                .ValueGeneratedOnAdd();

            builder.Property(oi => oi.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(oi => oi.Quantity)
                .IsRequired();

            builder.Property(oi => oi.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(oi => oi.Notes)
                .HasMaxLength(200);

            builder.HasOne(oi => oi.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrderId);
        }
    }
}