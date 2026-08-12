using MyWebApi.Api.Common;
using MyWebApi.Api.DBOperations;
using MyWebApi.Api.Entities;

namespace MyWebApi.Api.BookOperations.GetBookDetail;

public class GetBookDetailQuery
{
    private readonly BookStoreDbContext _dbContext;
    public int BookId { get; set; }


    public GetBookDetailQuery(BookStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public BookDetailViewModel Handle()
    {
        var book = _dbContext.Books.SingleOrDefault(x => x.Id == BookId);
        if (book is null)
        {
            throw new InvalidOperationException("Kitap bulunamadı!");
        }

        BookDetailViewModel vm = new BookDetailViewModel
        {
            Title = book.Title,
            Genre = ((GenreEnum)book.GenreId).ToString(),
            PageCount = book.PageCount,
            PublishDate = book.PublishDate.Date.ToString("dd/MM/yyyy")
        };
        return vm;
    }



}
public class BookDetailViewModel
{
    public string Title { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public int PageCount { get; set; }
    public string PublishDate { get; set; } = string.Empty;
}