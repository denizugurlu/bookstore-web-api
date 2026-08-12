using AutoMapper;
using MyWebApi.Api.DBOperations;

namespace MyWebApi.Api.BookOperations.GetBooks;

public class GetBooksQuery
{
    private readonly BookStoreDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetBooksQuery(BookStoreDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public List<BooksViewModel> Handle()
    {
        var bookList = _dbContext.Books.OrderBy(x => x.Id).ToList();
        List<BooksViewModel> vm = _mapper.Map<List<BooksViewModel>>(bookList);
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
