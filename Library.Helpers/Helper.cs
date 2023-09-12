using Library.Repository.Contracts;
using Library.Repository.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Helpers
{
    public static class Helper
    {
        public static void AddServices(this IServiceCollection service)
        {
            service.AddScoped<IGenreRepository, GenreRepository>();
            service.AddScoped<IAuthorRepository, AuthorRepository>();
            service.AddScoped<IBookRepository, BookRepository>();
            service.AddScoped<IFileRepository, FileRepository>();
            service.AddScoped<IAuthManager,AuthManager>();
        }
    }
}