using FluentAssertions;
using MyWebApi.Api.DBOperations;
using MyWebApi.Api.Entities;
using MyWebApi.Api.GenreOperations.DeleteGenre;
using MyWebApi.UnitTests.TestSetup;
using Xunit;

namespace MyWebApi.UnitTests.Application.GenreOperations.Commands.DeleteGenre;

public class DeleteGenreCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly BookStoreDbContext _context;

    public DeleteGenreCommandTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
    }

    // =========================================================================
    // TEST 1: Olmayan bir Genre silinmek istendiğinde hata vermeli
    // =========================================================================
    [Fact]
    public void WhenGenreIsNotFound_InvalidOperationException_ShouldBeReturn()
    {
        // ARRANGE
        var command = new DeleteGenreCommand(_context);
        command.GenreId = 99999;

        // ACT & ASSERT
        FluentActions
            .Invoking(() => command.Handle())
            .Should()
            .Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("Kitap türü bulunamadı.");
    }

    // =========================================================================
    // TEST 2: Var olan bir Genre silindiğinde DB'den kaldırılmalı
    // =========================================================================
    [Fact]
    public void WhenValidGenreIdIsGiven_Genre_ShouldBeDeleted()
    {
        // ARRANGE
        var genre = new Genre { Name = "Test_Delete_Genre" };
        _context.Genres.Add(genre);
        _context.SaveChanges();

        var command = new DeleteGenreCommand(_context);
        command.GenreId = genre.Id;

        // ACT
        FluentActions.Invoking(() => command.Handle()).Invoke();

        // ASSERT
        var deletedGenre = _context.Genres.SingleOrDefault(x => x.Id == genre.Id);
        deletedGenre.Should().BeNull();
    }
}
