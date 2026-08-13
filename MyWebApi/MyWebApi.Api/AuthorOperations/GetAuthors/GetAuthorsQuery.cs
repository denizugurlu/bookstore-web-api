using AutoMapper;
using MyWebApi.Api.DBOperations;

namespace MyWebApi.Api.AuthorOperations.GetAuthors;

public class GetAuthorsQuery
{
    private readonly BookStoreDbContext _context;
    private readonly IMapper _mapper;

    public GetAuthorsQuery(BookStoreDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public List<AuthorsViewModel> Handle()
    {
        var authors = _context.Authors.OrderBy(x => x.Id).ToList();
        List<AuthorsViewModel> returnObj = _mapper.Map<List<AuthorsViewModel>>(authors);
        return returnObj;
    }
}

public class AuthorsViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string DateOfBirth { get; set; } = string.Empty;
}
