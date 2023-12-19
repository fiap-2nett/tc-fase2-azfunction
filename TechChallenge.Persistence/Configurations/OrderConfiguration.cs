using Microsoft.EntityFrameworkCore;
using TechChallenge.Domain.Entities;
using TechChallenge.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TechChallenge.Persistence.Configurations
{
    internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("orders");

            builder.HasKey(order => order.Id);

            builder.Property(order => order.Id);                
            builder.OwnsOne(order => order.CustomerEmail, builder =>
            {
                builder.WithOwner();
                builder.Property(email => email.Value)
                    .HasColumnName(nameof(Order.CustomerEmail))
                    .HasMaxLength(Email.MaxLength)
                    .IsRequired();
            });
            builder.Property(order => order.Status);
            builder.Property(order => order.CreatedAt);
            builder.Property(order => order.LastUpdatedAt);

            builder.HasMany(order => order.Items)
                .WithOne()
                .HasForeignKey(orderItem => orderItem.OrderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
