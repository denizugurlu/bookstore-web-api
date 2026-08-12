using Microsoft.EntityFrameworkCore;
using MyWebApi.Api.Entities;

namespace MyWebApi.Api.DBOperations;

public class BookStoreDbContext : DbContext
{
    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) : base(options)
    {

    }
    public DbSet<Book> Books { get; set; }
}
