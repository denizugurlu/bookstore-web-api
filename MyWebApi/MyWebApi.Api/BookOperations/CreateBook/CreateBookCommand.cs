using MyWebApi.Api.DBOperations;
using MyWebApi.Api.Entities;

namespace MyWebApi.Api.BookOperations.CreateBook;

public class CreateBookCommand
{
    public CreateBookModel Model { get; set; } = null!;
    private readonly BookStoreDbContext _dbContext;

    public CreateBookCommand(BookStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Handle()
    {
        var book = _dbContext.Books.SingleOrDefault(x => x.Title == Model.Title);
        if (book is not null)
            throw new InvalidOperationException("Kitap zaten mevcut.");

        book = new Book
        {
            Title = Model.Title,
            GenreId = Model.GenreId,
            PageCount = Model.PageCount,
            PublishDate = Model.PublishDate
        };

        _dbContext.Books.Add(book);
        _dbContext.SaveChanges();
    }
}

public class CreateBookModel
{
    public string Title { get; set; } = string.Empty;
    public int GenreId { get; set; }
    public int PageCount { get; set; }
    public DateTime PublishDate { get; set; }
}
