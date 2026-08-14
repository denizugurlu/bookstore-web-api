using FluentAssertions;
using MyWebApi.Api.DBOperations;
using MyWebApi.Api.Entities;
using MyWebApi.Api.GenreOperations.UpdateGenre;
using MyWebApi.UnitTests.TestSetup;
using Xunit;

namespace MyWebApi.UnitTests.Application.GenreOperations.Commands.UpdateGenre;

public class UpdateGenreCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly BookStoreDbContext _context;

    public UpdateGenreCommandTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
    }

    // =========================================================================
    // TEST 1: Olmayan bir Genre güncellenmek istendiğinde hata vermeli
    // =========================================================================
    [Fact]
    public void WhenGenreIsNotFound_InvalidOperationException_ShouldBeReturn()
    {
        // ARRANGE
        var command = new UpdateGenreCommand(_context);
        command.GenreId = 99999;
        command.Model = new UpdateGenreModel { Name = "New Genre Name" };

        // ACT & ASSERT
        FluentActions
            .Invoking(() => command.Handle())
            .Should()
            .Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("Kitap türü bulunamadı.");
    }

    // =========================================================================
    // TEST 2: Başka bir türe ait aynı isim güncellenmek istendiğinde hata vermeli
    // =========================================================================
    [Fact]
    public void WhenAlreadyExistGenreNameIsGiven_InvalidOperationException_ShouldBeReturn()
    {
        // ARRANGE: DB'de iki tür oluşturuyoruz
        var genre1 = new Genre { Name = "Genre One" };
        var genre2 = new Genre { Name = "Genre Two" };
        _context.Genres.AddRange(genre1, genre2);
        _context.SaveChanges();

        // genre2'nin ismini genre1 ile aynı yapmaya çalışıyoruz
        var command = new UpdateGenreCommand(_context);
        command.GenreId = genre2.Id;
        command.Model = new UpdateGenreModel { Name = genre1.Name };

        // ACT & ASSERT
        FluentActions
            .Invoking(() => command.Handle())
            .Should()
            .Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("Aynı isimli bir kitap türü zaten mevcut.");
    }

    // =========================================================================
    // TEST 3: Geçerli girdiler verildiğinde tür bilgileri güncellenmeli
    // =========================================================================
    [Fact]
    public void WhenValidInputsAreGiven_Genre_ShouldBeUpdated()
    {
        // ARRANGE
        var genre = new Genre { Name = "Old Genre Name", IsActive = true };
        _context.Genres.Add(genre);
        _context.SaveChanges();

        var command = new UpdateGenreCommand(_context);
        command.GenreId = genre.Id;
        var model = new UpdateGenreModel { Name = "Updated Genre Name", IsActive = false };
        command.Model = model;

        // ACT
        FluentActions.Invoking(() => command.Handle()).Invoke();

        // ASSERT
        var updatedGenre = _context.Genres.SingleOrDefault(x => x.Id == genre.Id);
        updatedGenre.Should().NotBeNull();
        updatedGenre!.Name.Should().Be(model.Name);
        updatedGenre.IsActive.Should().BeFalse();
    }
}
