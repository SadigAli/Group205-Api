using FirstApi.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FirstApi.Entities
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {   
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration<Product>(new ProductConfiguration());
            //modelBuilder.Entity<Product>().HasData(
            //    new Product
            //    {
            //        Id = 1,
            //        Title = "Product 1",
            //        Description = "Description",
            //        Price = 10.05,
            //        Count = 10,
            //    },
            //    new Product
            //    {
            //        Id = 2,
            //        Title = "Product 2",
            //        Description = "Description",
            //        Price = 15.5,
            //        Count = 12,
            //    }
            //);

        }
    }
}
