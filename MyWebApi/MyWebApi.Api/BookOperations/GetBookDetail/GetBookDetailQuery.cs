using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyWebApi.Api.DBOperations;

namespace MyWebApi.Api.BookOperations.GetBookDetail;

public class GetBookDetailQuery
{
    private readonly BookStoreDbContext _dbContext;
    private readonly IMapper _mapper;
    public int BookId { get; set; }

    public GetBookDetailQuery(BookStoreDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public BookDetailViewModel Handle()
    {
        var book = _dbContext.Books.Include(x => x.Genre).Include(x => x.Author).SingleOrDefault(x => x.Id == BookId);
        if (book is null)
            throw new InvalidOperationException("Kitap bulunamadı!");

        BookDetailViewModel vm = _mapper.Map<BookDetailViewModel>(book);
        return vm;
    }
}

public class BookDetailViewModel
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public int PageCount { get; set; }
    public string PublishDate { get; set; } = string.Empty;
}