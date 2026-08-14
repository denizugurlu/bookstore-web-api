using FluentAssertions;
using MyWebApi.Api.BookOperations.UpdateBook;
using MyWebApi.Api.DBOperations;
using MyWebApi.Api.Entities;
using MyWebApi.UnitTests.TestSetup;
using Xunit;

namespace MyWebApi.UnitTests.Application.BookOperations.Commands.UpdateBook;

public class UpdateBookCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly BookStoreDbContext _context;

    public UpdateBookCommandTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
    }

    // =========================================================================
    // TEST 1: Veritabanında olmayan kitap güncellenmek istendiğinde hata vermeli
    // =========================================================================
    [Fact]
    public void WhenBookIsNotFound_InvalidOperationException_ShouldBeReturn()
    {
        // ARRANGE
        var command = new UpdateBookCommand(_context);
        command.BookId = 99999;
        command.Model = new UpdateBookModel
        {
            Title = "Updated Book Title",
            GenreId = 1,
            AuthorId = 1
        };

        // ACT & ASSERT
        FluentActions
            .Invoking(() => command.Handle())
            .Should()
            .Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("Güncellenecek kitap bulunamadı!");
    }

    // =========================================================================
    // TEST 2: Geçerli bilgiler verildiğinde kitap bilgileri güncellenmeli
    // =========================================================================
    [Fact]
    public void WhenValidInputsAreGiven_Book_ShouldBeUpdated()
    {
        // ARRANGE
        var book = new Book
        {
            Title = "Original Title",
            PageCount = 200,
            PublishDate = new DateTime(2000, 1, 1),
            GenreId = 1,
            AuthorId = 1
        };
        _context.Books.Add(book);
        _context.SaveChanges();

        var command = new UpdateBookCommand(_context);
        command.BookId = book.Id;
        var model = new UpdateBookModel
        {
            Title = "New Updated Title",
            GenreId = 2,
            AuthorId = 2
        };
        command.Model = model;

        // ACT
        FluentActions.Invoking(() => command.Handle()).Invoke();

        // ASSERT
        var updatedBook = _context.Books.SingleOrDefault(x => x.Id == book.Id);
        updatedBook.Should().NotBeNull();
        updatedBook!.Title.Should().Be(model.Title);
        updatedBook.GenreId.Should().Be(model.GenreId);
        updatedBook.AuthorId.Should().Be(model.AuthorId);
    }
}
