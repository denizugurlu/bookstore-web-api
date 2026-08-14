using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MyWebApi.Api.BookOperations.DeleteBook;
using MyWebApi.Api.DBOperations;
using MyWebApi.Api.Entities;
using MyWebApi.UnitTests.TestSetup;
using Xunit;
namespace MyWebApi.UnitTests.Application.BookOperations.Commands.DeleteBook;

public class DeleteBookCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly BookStoreDbContext _context;
    // TestFixture'dan RAM veritabanımızı alıyoruz
    public DeleteBookCommandTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
    }

    [Fact]
    public void WhenBookIsNotFound_InvalidOperationException_ShouldBeReturn()
    {
        var command = new DeleteBookCommand(_context);
        command.BookId = 99999;


        FluentActions.Invoking(() => command.Handle()).Should().Throw<InvalidOperationException>().And.Message.Should().Be("Silinecek kitap bulunamadı.");


    }

    [Fact]
    public void WhenValidBookIdIsGiven_Book_ShouldBeDeleted()
    {
        var book = new Book()
        {
            Title = "Test_DeleteBook",
            PageCount = 250,
            PublishDate = new DateTime(2000, 01, 01),
            GenreId = 1,
            AuthorId = 1
        };

        _context.Books.Add(book);
        _context.SaveChanges();

        var command = new DeleteBookCommand(_context);
        command.BookId = book.Id;

        FluentActions.Invoking(() => command.Handle()).Invoke();

        var deletedBook = _context.Books.SingleOrDefault(x => x.Id == book.Id);

        deletedBook.Should().BeNull();



    }






}

