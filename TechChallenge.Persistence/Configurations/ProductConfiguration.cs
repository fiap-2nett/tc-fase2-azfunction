using Microsoft.EntityFrameworkCore;
using TechChallenge.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TechChallenge.Persistence.Configurations
{
    internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("products");

            builder.HasKey(product => product.Id);

            builder.Property(product => product.Id).ValueGeneratedNever();
            builder.Property(product => product.Name).HasMaxLength(255).IsRequired();
            builder.Property(product => product.Price);
            builder.Property(product => product.Quantity);
        }
    }
}
