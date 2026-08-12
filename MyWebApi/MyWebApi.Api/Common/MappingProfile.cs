using AutoMapper;
using MyWebApi.Api.BookOperations.CreateBook;
using MyWebApi.Api.BookOperations.GetBookDetail;
using MyWebApi.Api.BookOperations.GetBooks;
using MyWebApi.Api.Common;
using MyWebApi.Api.Entities;

namespace MyWebApi.Api.Common;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // CreateBookModel -> Book mapping
        CreateMap<CreateBookModel, Book>();

        // Book -> BookDetailViewModel mapping
        CreateMap<Book, BookDetailViewModel>()
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => ((GenreEnum)src.GenreId).ToString()))
            .ForMember(dest => dest.PublishDate, opt => opt.MapFrom(src => src.PublishDate.Date.ToString("dd/MM/yyyy")));

        // Book -> BooksViewModel mapping
        CreateMap<Book, BooksViewModel>()
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => ((GenreEnum)src.GenreId).ToString()))
            .ForMember(dest => dest.PublishDate, opt => opt.MapFrom(src => src.PublishDate.Date.ToString("dd/MM/yyyy")));
    }
}
