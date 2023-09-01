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
        public DbSet<Category> Categories { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.ApplyConfiguration<Product>(new ProductConfiguration());
            //modelBuilder.ApplyConfiguration<Category>(new CategoryConfiguration());
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
