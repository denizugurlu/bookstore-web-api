using MyWebApi.Api.Common;
using MyWebApi.Api.DBOperations;

namespace MyWebApi.Api.BookOperations.GetBooks;

public class GetBooksQuery
{
    private readonly BookStoreDbContext _dbContext;

    public GetBooksQuery(BookStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<BooksViewModel> Handle()
    {
        var bookList = _dbContext.Books.OrderBy(x => x.Id).ToList();

        List<BooksViewModel> vm = new List<BooksViewModel>();
        foreach (var book in bookList)
        {
            vm.Add(new BooksViewModel
            {
                Title = book.Title,
                Genre = ((GenreEnum)book.GenreId).ToString(),
                PageCount = book.PageCount,
                PublishDate = book.PublishDate.Date.ToString("dd/MM/yyyy")
            });
        }

        return vm;
    }
}

public class BooksViewModel
{
    public string Title { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public int PageCount { get; set; }
    public string PublishDate { get; set; } = string.Empty;
}
