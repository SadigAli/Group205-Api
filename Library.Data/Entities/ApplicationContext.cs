using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Entities
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            
        }

        public DbSet<Genre> Genres { get; set; }
    }

    public class DbContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        public ApplicationContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json")
                            .Build();
            var connectionStrings = config.GetConnectionString("MyConnection");
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseSqlServer(connectionStrings);
            return new ApplicationContext(optionsBuilder.Options);
        }
    }
}
