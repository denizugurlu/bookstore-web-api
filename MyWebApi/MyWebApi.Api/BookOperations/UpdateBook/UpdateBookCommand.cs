using MyWebApi.Api.DBOperations;

namespace MyWebApi.Api.BookOperations.UpdateBook;

public class UpdateBookCommand
{
    private readonly BookStoreDbContext _dbContext;
    public int BookId { get; set; }
    public UpdateBookModel Model { get; set; } = null!;

    public UpdateBookCommand(BookStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Handle()
    {
        var book = _dbContext.Books.SingleOrDefault(x => x.Id == BookId);
        if (book is null)
            throw new InvalidOperationException("Güncellenecek kitap bulunamadı!");

        book.GenreId = Model.GenreId != default ? Model.GenreId : book.GenreId;
        book.AuthorId = Model.AuthorId != default ? Model.AuthorId : book.AuthorId;
        book.Title = Model.Title != default ? Model.Title : book.Title;

        _dbContext.SaveChanges();
    }
}

public class UpdateBookModel
{
    public string Title { get; set; } = string.Empty;
    public int GenreId { get; set; }
    public int AuthorId { get; set; }
}
