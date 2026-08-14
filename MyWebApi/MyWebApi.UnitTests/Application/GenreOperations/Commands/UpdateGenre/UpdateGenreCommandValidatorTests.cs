using FluentAssertions;
using MyWebApi.Api.GenreOperations.UpdateGenre;
using MyWebApi.UnitTests.TestSetup;
using Xunit;

namespace MyWebApi.UnitTests.Application.GenreOperations.Commands.UpdateGenre;

public class UpdateGenreCommandValidatorTests : IClassFixture<CommonTestFixture>
{
    // =========================================================================
    // TEST 1: Geçersiz GenreId veya 4 karakterden kısa isim verildiğinde hata dönmeli
    // =========================================================================
    [Theory]
    [InlineData(0, "History")] // GenreId 0 olamaz
    [InlineData(-1, "History")]
    [InlineData(1, "abc")]     // 3 karakter (minimum 4 olmalı)
    [InlineData(1, "a")]
    public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnErrors(int genreId, string name)
    {
        // ARRANGE
        var command = new UpdateGenreCommand(null!);
        command.GenreId = genreId;
        command.Model = new UpdateGenreModel { Name = name };

        // ACT
        var validator = new UpdateGenreCommandValidator();
        var result = validator.Validate(command);

        // ASSERT
        result.Errors.Count.Should().BeGreaterThan(0);
    }

    // =========================================================================
    // TEST 2: Geçerli girdiler verildiğinde hata dönmemeli
    // =========================================================================
    [Theory]
    [InlineData(1, "History")]
    [InlineData(1, "")] // Boş bırakılırsa isim güncellenmez, validatordan geçer
    public void WhenValidInputsAreGiven_Validator_ShouldNotBeReturnError(int genreId, string name)
    {
        // ARRANGE
        var command = new UpdateGenreCommand(null!);
        command.GenreId = genreId;
        command.Model = new UpdateGenreModel { Name = name };

        // ACT
        var validator = new UpdateGenreCommandValidator();
        var result = validator.Validate(command);

        // ASSERT
        result.Errors.Count.Should().Be(0);
    }
}
