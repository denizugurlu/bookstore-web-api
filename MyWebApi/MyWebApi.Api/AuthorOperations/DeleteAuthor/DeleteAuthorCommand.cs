using MyWebApi.Api.DBOperations;

namespace MyWebApi.Api.AuthorOperations.DeleteAuthor;

public class DeleteAuthorCommand
{
    public int AuthorId { get; set; }
    private readonly BookStoreDbContext _context;

    public DeleteAuthorCommand(BookStoreDbContext context)
    {
        _context = context;
    }

    public void Handle()
    {
        var author = _context.Authors.SingleOrDefault(x => x.Id == AuthorId);
        if (author is null)
            throw new InvalidOperationException("Yazar bulunamadı.");

        var hasBooks = _context.Books.Any(x => x.AuthorId == AuthorId);
        if (hasBooks)
            throw new InvalidOperationException("Yazara ait kayıtlı kitap bulunmaktadır. Önce yazarın kitaplarını silmelisiniz.");

        _context.Authors.Remove(author);
        _context.SaveChanges();
    }
}
