using FirstApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FirstApi.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData
                (
                    new Category
                    {
                        Id = 1,
                        Name = "Electronic"
                    },
                    new Category
                    {
                        Id = 2,
                        Name = "Heyvan"
                    },
                    new Category
                    {
                        Id = 3,
                        Name = "Kitab"
                    }
                );
        }
    }
}
