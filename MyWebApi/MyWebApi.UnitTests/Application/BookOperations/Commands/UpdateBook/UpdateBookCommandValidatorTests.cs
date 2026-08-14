using FluentAssertions;
using MyWebApi.Api.BookOperations.UpdateBook;
using MyWebApi.UnitTests.TestSetup;
using Xunit;

namespace MyWebApi.UnitTests.Application.BookOperations.Commands.UpdateBook;

public class UpdateBookCommandValidatorTests : IClassFixture<CommonTestFixture>
{
    // =========================================================================
    // TEST 1: Geçersiz BookId veya kurallara uymayan model verildiğinde hata dönmeli
    // =========================================================================
    [Theory]
    [InlineData(0, "Lord of The Rings", 1, 1)]  // BookId 0 olamaz
    [InlineData(-1, "Lord of The Rings", 1, 1)] // BookId negatif olamaz
    [InlineData(1, "Lor", 1, 1)]                // Title doluysa en az 4 karakter olmalı
    [InlineData(1, "Lord of The Rings", -1, 1)] // GenreId negatif olamaz
    [InlineData(1, "Lord of The Rings", 1, -1)] // AuthorId negatif olamaz
    public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnErrors(
        int bookId, string title, int genreId, int authorId)
    {
        // ARRANGE
        var command = new UpdateBookCommand(null!);
        command.BookId = bookId;
        command.Model = new UpdateBookModel
        {
            Title = title,
            GenreId = genreId,
            AuthorId = authorId
        };

        // ACT
        var validator = new UpdateBookCommandValidator();
        var result = validator.Validate(command);

        // ASSERT
        result.Errors.Count.Should().BeGreaterThan(0);
    }

    // =========================================================================
    // TEST 2: Geçerli girdiler verildiğinde hata dönmemeli (Happy Path)
    // =========================================================================
    [Theory]
    [InlineData(1, "Lord Of The Rings", 1, 1)]
    [InlineData(1, "", 0, 0)] // Boş bırakılan alanlar güncellenmez, kuralı bozmaz
    public void WhenValidInputsAreGiven_Validator_ShouldNotBeReturnError(
        int bookId, string title, int genreId, int authorId)
    {
        // ARRANGE
        var command = new UpdateBookCommand(null!);
        command.BookId = bookId;
        command.Model = new UpdateBookModel
        {
            Title = title,
            GenreId = genreId,
            AuthorId = authorId
        };

        // ACT
        var validator = new UpdateBookCommandValidator();
        var result = validator.Validate(command);

        // ASSERT
        result.Errors.Count.Should().Be(0);
    }
}
