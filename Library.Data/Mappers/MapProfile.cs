using AutoMapper;
using Library.Data.DTOs.Author;
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
        }
    }
}
