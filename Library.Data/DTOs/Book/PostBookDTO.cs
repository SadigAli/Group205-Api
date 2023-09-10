using FluentValidation;
using Library.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.DTOs.Book
{
    public class PostBookDTO
    {
        public IFormFile? File { get; set; }
        public int? Page { get; set; }
        public string? Name { get; set; }
        public int? GenreId { get; set; }
        public List<int>? AuthorIds { get; set; }
    }

    public class BookValidator : AbstractValidator<PostBookDTO>
    {
        private readonly ApplicationContext _context;
        public BookValidator(ApplicationContext context)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Enter book name.")
                .MaximumLength(100).WithMessage("Book's length must be less than 100 symbols.")
                .MinimumLength(2).WithMessage("Book's length must be greater than 2 symbols");

            RuleFor(x => x.GenreId)
                .NotNull()
                .GreaterThanOrEqualTo(1)
                .Must(genreId => _context.Genres.Any(x => x.Id == genreId))
                .WithMessage("Genre doesn't exists");

            RuleFor(x => x.File)
                .Must(x => x == null || x.Length/1024 <= 256).WithMessage("File's length must be less than 256kb")
                .Must(x => x == null || x.ContentType.Contains("image")).WithMessage("File's format must be an image");

            RuleFor(x => x.Page)
                .NotNull()
                .GreaterThan(20);

            _context = context;

        }
    }
}
