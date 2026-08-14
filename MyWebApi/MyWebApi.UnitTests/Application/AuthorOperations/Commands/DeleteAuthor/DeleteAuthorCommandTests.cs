using FluentAssertions;
using MyWebApi.Api.AuthorOperations.DeleteAuthor;
using MyWebApi.Api.DBOperations;
using MyWebApi.Api.Entities;
using MyWebApi.UnitTests.TestSetup;
using Xunit;

namespace MyWebApi.UnitTests.Application.AuthorOperations.Commands.DeleteAuthor;

public class DeleteAuthorCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly BookStoreDbContext _context;

    public DeleteAuthorCommandTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
    }

    // =========================================================================
    // TEST 1: Olmayan bir yazar silinmek istendiğinde hata fırlatmalı
    // =========================================================================
    [Fact]
    public void WhenAuthorIsNotFound_InvalidOperationException_ShouldBeReturn()
    {
        // ARRANGE
        var command = new DeleteAuthorCommand(_context);
        command.AuthorId = 99999;

        // ACT & ASSERT
        FluentActions
            .Invoking(() => command.Handle())
            .Should()
            .Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("Yazar bulunamadı.");
    }

    // =========================================================================
    // TEST 2: Kitabı olan bir yazar silinmek istendiğinde hata fırlatmalı
    // =========================================================================
    [Fact]
    public void WhenAuthorHasBooks_InvalidOperationException_ShouldBeReturn()
    {
        // ARRANGE: Yazar ve ona bağlı bir kitap ekliyoruz
        var author = new Author { Name = "Author_With_Book", Surname = "Test", DateOfBirth = new DateTime(1980, 1, 1) };
        _context.Authors.Add(author);
        _context.SaveChanges();

        var book = new Book { Title = "Book_Of_Author", PageCount = 100, GenreId = 1, AuthorId = author.Id, PublishDate = new DateTime(2000, 1, 1) };
        _context.Books.Add(book);
        _context.SaveChanges();

        var command = new DeleteAuthorCommand(_context);
        command.AuthorId = author.Id;

        // ACT & ASSERT
        FluentActions
            .Invoking(() => command.Handle())
            .Should()
            .Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("Yazara ait kayıtlı kitap bulunmaktadır. Önce yazarın kitaplarını silmelisiniz.");
    }

    // =========================================================================
    // TEST 3: Kitabı olmayan geçerli yazar silindiğinde başarıyla DB'den kaldırılmalı
    // =========================================================================
    [Fact]
    public void WhenValidAuthorIdIsGiven_Author_ShouldBeDeleted()
    {
        // ARRANGE: Kitapsız bağımsız bir yazar ekliyoruz
        var author = new Author { Name = "Free_Author", Surname = "Test", DateOfBirth = new DateTime(1985, 5, 5) };
        _context.Authors.Add(author);
        _context.SaveChanges();

        var command = new DeleteAuthorCommand(_context);
        command.AuthorId = author.Id;

        // ACT
        FluentActions.Invoking(() => command.Handle()).Invoke();

        // ASSERT
        var deletedAuthor = _context.Authors.SingleOrDefault(x => x.Id == author.Id);
        deletedAuthor.Should().BeNull();
    }
}
