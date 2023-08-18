using FirstApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FirstApi.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData(
                new Product
                {
                    Id = 1,
                    Title = "Product 1",
                    Description = "Description",
                    Price = 10.05,
                    Count = 10,
                },
                new Product
                {
                    Id = 2,
                    Title = "Product 2",
                    Description = "Description",
                    Price = 15.5,
                    Count = 12,
                }
            );
        }
    }
}
