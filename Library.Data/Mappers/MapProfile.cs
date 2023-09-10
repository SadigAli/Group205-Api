using AutoMapper;
using Library.Data.DTOs.Author;
using Library.Data.DTOs.Book;
using Library.Data.DTOs.Genre;
using Library.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Mappers
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Genre, GenreGetDTO>().ReverseMap();
            CreateMap<GenrePostDTO, Genre>();

            CreateMap<Author, AuthorGetDTO>();
            CreateMap<AuthorPostDTO, Author>();

            CreateMap<Book, GetBookDTO>()
                .ForMember(dest => dest.Image, src => src.MapFrom(x => "https://localhost:7071/uploads/books/" + x.Image))
                .ForMember(dest => dest.Authors, src => src.MapFrom(x => x.BookAuthors.Select(x => x.Author.GetFullName()).ToList()))
                .ForMember(dest=>dest.Genre, src=>src.MapFrom(x=>x.Genre.Name));
            CreateMap<PostBookDTO, Book>();

        }
    }
}
