using Microsoft.EntityFrameworkCore;
using MyWebApi.Api.Entities;

namespace MyWebApi.Api.DBOperations;

public static class DataGenerator
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new BookStoreDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<BookStoreDbContext>>());

        if (context.Books.Any())
        {
            return; // Zaten veri varsa tekrar ekleme
        }

        context.Genres.AddRange(
            new Genre
            {
                Name = "Personal Growth"
            },
            new Genre
            {
                Name = "Science Fiction"
            },
            new Genre
            {
                Name = "Noval"
            }
        );

        context.Authors.AddRange(
            new Author
            {
                Name = "Eric",
                Surname = "Ries",
                DateOfBirth = new DateTime(1978, 09, 22)
            },
            new Author
            {
                Name = "Frank",
                Surname = "Herbert",
                DateOfBirth = new DateTime(1920, 10, 08)
            },
            new Author
            {
                Name = "George",
                Surname = "Orwell",
                DateOfBirth = new DateTime(1903, 06, 25)
            }
        );

        context.Books.AddRange(
            new Book
            {
                Title = "Lean Startup",
                GenreId = 1, // PersonalGrowth
                AuthorId = 1, // Eric Ries
                PageCount = 200,
                PublishDate = new DateTime(2001, 06, 12)
            },
            new Book
            {
                Title = "Dune",
                GenreId = 2, // ScienceFiction
                AuthorId = 2, // Frank Herbert
                PageCount = 540,
                PublishDate = new DateTime(2001, 12, 21)
            },
            new Book
            {
                Title = "1984",
                GenreId = 3, // Noval
                AuthorId = 3, // George Orwell
                PageCount = 328,
                PublishDate = new DateTime(1949, 06, 08)
            }
        );

        context.SaveChanges();
    }
}
