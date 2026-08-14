using MyWebApi.Api.DBOperations;
using MyWebApi.Api.Entities;

namespace MyWebApi.UnitTests.TestSetup;

public static class Books
{
    // DbContext'e genişletme (extension) metodu: Test için örnek kitaplar ekler
    public static void AddBooks(this BookStoreDbContext context)
    {
        context.Books.AddRange(
            new Book
            {
                Title = "Lean Startup",
                GenreId = 1,
                AuthorId = 1,
                PageCount = 200,
                PublishDate = new DateTime(2001, 06, 12)
            },
            new Book
            {
                Title = "Dune",
                GenreId = 2,
                AuthorId = 2,
                PageCount = 540,
                PublishDate = new DateTime(2001, 12, 21)
            },
            new Book
            {
                Title = "1984",
                GenreId = 3,
                AuthorId = 3,
                PageCount = 328,
                PublishDate = new DateTime(1949, 06, 08)
            }
        );
    }
}
