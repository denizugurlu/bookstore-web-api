using AutoMapper;
using MyWebApi.Api.BookOperations.CreateBook;
using MyWebApi.Api.BookOperations.GetBookDetail;
using MyWebApi.Api.BookOperations.GetBooks;
using MyWebApi.Api.Common;
using MyWebApi.Api.Entities;
using MyWebApi.Api.GenreOperations.GetGenreDetail;
using MyWebApi.Api.GenreOperations.GetGenres;

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

        // Genre -> GenresViewModel mapping
        CreateMap<Genre, GenresViewModel>();

        // Genre -> GenreDetailViewModel mapping
        CreateMap<Genre, GenreDetailViewModel>();
    }
}
