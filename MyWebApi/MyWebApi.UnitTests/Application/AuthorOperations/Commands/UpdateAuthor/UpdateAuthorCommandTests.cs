using FluentAssertions;
using MyWebApi.Api.AuthorOperations.UpdateAuthor;
using MyWebApi.Api.DBOperations;
using MyWebApi.Api.Entities;
using MyWebApi.UnitTests.TestSetup;
using Xunit;

namespace MyWebApi.UnitTests.Application.AuthorOperations.Commands.UpdateAuthor;

public class UpdateAuthorCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly BookStoreDbContext _context;

    public UpdateAuthorCommandTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
    }

    // =========================================================================
    // TEST 1: Olmayan bir yazar güncellenmek istendiğinde hata fırlatmalı
    // =========================================================================
    [Fact]
    public void WhenAuthorIsNotFound_InvalidOperationException_ShouldBeReturn()
    {
        // ARRANGE
        var command = new UpdateAuthorCommand(_context);
        command.AuthorId = 99999;
        command.Model = new UpdateAuthorModel { Name = "New Name", Surname = "New Surname" };

        // ACT & ASSERT
        FluentActions
            .Invoking(() => command.Handle())
            .Should()
            .Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("Yazar bulunamadı.");
    }

    // =========================================================================
    // TEST 2: Başka bir yazara ait aynı isim ve soyisim girildiğinde hata vermeli
    // =========================================================================
    [Fact]
    public void WhenAlreadyExistAuthorNameIsGiven_InvalidOperationException_ShouldBeReturn()
    {
        // ARRANGE: DB'de iki farklı yazar oluşturuyoruz
        var author1 = new Author { Name = "Franz", Surname = "Kafka", DateOfBirth = new DateTime(1883, 7, 3) };
        var author2 = new Author { Name = "Albert", Surname = "Camus", DateOfBirth = new DateTime(1913, 11, 7) };
        _context.Authors.AddRange(author1, author2);
        _context.SaveChanges();

        // author2'nin adını ve soyadını author1 ile aynı yapmaya çalışıyoruz
        var command = new UpdateAuthorCommand(_context);
        command.AuthorId = author2.Id;
        command.Model = new UpdateAuthorModel { Name = author1.Name, Surname = author1.Surname };

        // ACT & ASSERT
        FluentActions
            .Invoking(() => command.Handle())
            .Should()
            .Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("Aynı isim ve soyisimde başka bir yazar mevcut.");
    }

    // =========================================================================
    // TEST 3: Geçerli girdiler verildiğinde yazar bilgileri güncellenmeli
    // =========================================================================
    [Fact]
    public void WhenValidInputsAreGiven_Author_ShouldBeUpdated()
    {
        // ARRANGE
        var author = new Author { Name = "Old Author Name", Surname = "Old Surname", DateOfBirth = new DateTime(1960, 1, 1) };
        _context.Authors.Add(author);
        _context.SaveChanges();

        var command = new UpdateAuthorCommand(_context);
        command.AuthorId = author.Id;
        var model = new UpdateAuthorModel
        {
            Name = "Updated Author Name",
            Surname = "Updated Surname",
            DateOfBirth = new DateTime(1965, 5, 5)
        };
        command.Model = model;

        // ACT
        FluentActions.Invoking(() => command.Handle()).Invoke();

        // ASSERT
        var updatedAuthor = _context.Authors.SingleOrDefault(x => x.Id == author.Id);
        updatedAuthor.Should().NotBeNull();
        updatedAuthor!.Name.Should().Be(model.Name);
        updatedAuthor.Surname.Should().Be(model.Surname);
        updatedAuthor.DateOfBirth.Should().Be(model.DateOfBirth);
    }
}
