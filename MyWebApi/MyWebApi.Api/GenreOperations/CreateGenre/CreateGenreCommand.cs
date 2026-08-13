using MyWebApi.Api.DBOperations;
using MyWebApi.Api.Entities;

namespace MyWebApi.Api.GenreOperations.CreateGenre;

public class CreateGenreCommand
{
    public CreateGenreModel Model { get; set; } = null!;
    private readonly BookStoreDbContext _context;

    public CreateGenreCommand(BookStoreDbContext context)
    {
        _context = context;
    }

    public void Handle()
    {
        var genre = _context.Genres.SingleOrDefault(x => x.Name.ToLower() == Model.Name.ToLower());
        if (genre is not null)
            throw new InvalidOperationException("Kitap türü zaten mevcut.");

        genre = new Genre();
        genre.Name = Model.Name;

        _context.Genres.Add(genre);
        _context.SaveChanges();
    }
}

public class CreateGenreModel
{
    public string Name { get; set; } = string.Empty;
}
