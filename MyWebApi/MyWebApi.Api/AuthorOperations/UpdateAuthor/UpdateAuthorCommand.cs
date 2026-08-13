using MyWebApi.Api.DBOperations;

namespace MyWebApi.Api.AuthorOperations.UpdateAuthor;

public class UpdateAuthorCommand
{
    public int AuthorId { get; set; }
    public UpdateAuthorModel Model { get; set; } = null!;
    private readonly BookStoreDbContext _context;

    public UpdateAuthorCommand(BookStoreDbContext context)
    {
        _context = context;
    }

    public void Handle()
    {
        var author = _context.Authors.SingleOrDefault(x => x.Id == AuthorId);
        if (author is null)
            throw new InvalidOperationException("Yazar bulunamadı.");

        var duplicateAuthor = _context.Authors.SingleOrDefault(x =>
            x.Name.ToLower() == Model.Name.ToLower() &&
            x.Surname.ToLower() == Model.Surname.ToLower() &&
            x.Id != AuthorId);

        if (duplicateAuthor is not null)
            throw new InvalidOperationException("Aynı isim ve soyisimde başka bir yazar mevcut.");

        author.Name = string.IsNullOrEmpty(Model.Name.Trim()) ? author.Name : Model.Name;
        author.Surname = string.IsNullOrEmpty(Model.Surname.Trim()) ? author.Surname : Model.Surname;
        author.DateOfBirth = Model.DateOfBirth != default ? Model.DateOfBirth : author.DateOfBirth;

        _context.SaveChanges();
    }
}

public class UpdateAuthorModel
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
}
