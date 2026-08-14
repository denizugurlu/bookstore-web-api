using FluentAssertions;
using MyWebApi.Api.DBOperations;
using MyWebApi.Api.Entities;
using MyWebApi.Api.GenreOperations.CreateGenre;
using MyWebApi.UnitTests.TestSetup;
using Xunit;

namespace MyWebApi.UnitTests.Application.GenreOperations.Commands.CreateGenre;

public class CreateGenreCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly BookStoreDbContext _context;

    public CreateGenreCommandTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
    }

    // =========================================================================
    // TEST 1: Zaten var olan bir tür adı girildiğinde hata vermeli
    // =========================================================================
    [Fact]
    public void WhenAlreadyExistGenreNameIsGiven_InvalidOperationException_ShouldBeReturn()
    {
        // ARRANGE: DB'ye tür ekliyoruz
        var genre = new Genre { Name = "Test_Exist_Genre" };
        _context.Genres.Add(genre);
        _context.SaveChanges();

        var command = new CreateGenreCommand(_context);
        command.Model = new CreateGenreModel { Name = genre.Name };

        // ACT & ASSERT
        FluentActions
            .Invoking(() => command.Handle())
            .Should()
            .Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("Kitap türü zaten mevcut.");
    }

    // =========================================================================
    // TEST 2: Geçerli tür adı verildiğinde tür DB'ye başarıyla kaydedilmeli
    // =========================================================================
    [Fact]
    public void WhenValidInputsAreGiven_Genre_ShouldBeCreated()
    {
        // ARRANGE
        var command = new CreateGenreCommand(_context);
        var model = new CreateGenreModel { Name = "Philosophy" };
        command.Model = model;

        // ACT
        FluentActions.Invoking(() => command.Handle()).Invoke();

        // ASSERT
        var savedGenre = _context.Genres.SingleOrDefault(x => x.Name == model.Name);
        savedGenre.Should().NotBeNull();
        savedGenre!.IsActive.Should().BeTrue();
    }
}
