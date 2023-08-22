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
                    Title = "It",
                    Description = "Adi Toplandir",
                    Price = 800,
                    Count = 1,
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
