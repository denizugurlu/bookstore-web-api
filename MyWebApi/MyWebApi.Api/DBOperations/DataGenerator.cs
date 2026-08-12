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

        context.Books.AddRange(
            new Book
            {
                Title = "Lean Startup",
                GenreId = 1, // PersonalGrowth
                PageCount = 200,
                PublishDate = new DateTime(2001, 06, 12)
            },
            new Book
            {
                Title = "Dune",
                GenreId = 2, // ScienceFiction
                PageCount = 540,
                PublishDate = new DateTime(2001, 12, 21)
            },
            new Book
            {
                Title = "1984",
                GenreId = 3, // Noval
                PageCount = 328,
                PublishDate = new DateTime(1949, 06, 08)
            }
        );

        context.SaveChanges();
    }
}
