using Microsoft.EntityFrameworkCore;
using TechChallenge.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TechChallenge.Persistence.Configurations
{
    internal sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("orderitems");

            builder.HasKey(orderItem => orderItem.Id);

            builder.Property(orderItem => orderItem.Id);
            builder.Property(orderItem => orderItem.OrderId);
            builder.Property(orderItem => orderItem.ProductId);
            builder.Property(orderItem => orderItem.Price);
            builder.Property(orderItem => orderItem.Quantity);

            builder.HasOne<Product>()
                .WithMany()
                .HasForeignKey(orderItem => orderItem.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
