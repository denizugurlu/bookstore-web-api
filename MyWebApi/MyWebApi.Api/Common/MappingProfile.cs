using AutoMapper;
using MyWebApi.Api.AuthorOperations.CreateAuthor;
using MyWebApi.Api.AuthorOperations.GetAuthorDetail;
using MyWebApi.Api.AuthorOperations.GetAuthors;
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
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre != null ? src.Genre.Name : ((GenreEnum)src.GenreId).ToString()))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author != null ? $"{src.Author.Name} {src.Author.Surname}" : string.Empty))
            .ForMember(dest => dest.PublishDate, opt => opt.MapFrom(src => src.PublishDate.Date.ToString("dd/MM/yyyy")));

        // Book -> BooksViewModel mapping
        CreateMap<Book, BooksViewModel>()
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre != null ? src.Genre.Name : ((GenreEnum)src.GenreId).ToString()))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author != null ? $"{src.Author.Name} {src.Author.Surname}" : string.Empty))
            .ForMember(dest => dest.PublishDate, opt => opt.MapFrom(src => src.PublishDate.Date.ToString("dd/MM/yyyy")));

        // Genre -> GenresViewModel mapping
        CreateMap<Genre, GenresViewModel>();

        // Genre -> GenreDetailViewModel mapping
        CreateMap<Genre, GenreDetailViewModel>();

        // CreateAuthorModel -> Author mapping
        CreateMap<CreateAuthorModel, Author>();

        // Author -> AuthorsViewModel mapping
        CreateMap<Author, AuthorsViewModel>()
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.Date.ToString("dd/MM/yyyy")));

        // Author -> AuthorDetailViewModel mapping
        CreateMap<Author, AuthorDetailViewModel>()
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.Date.ToString("dd/MM/yyyy")));
    }
}
