using FluentAssertions;
using MyWebApi.Api.GenreOperations.DeleteGenre;
using MyWebApi.UnitTests.TestSetup;
using Xunit;

namespace MyWebApi.UnitTests.Application.GenreOperations.Commands.DeleteGenre;

public class DeleteGenreCommandValidatorTests : IClassFixture<CommonTestFixture>
{
    // =========================================================================
    // TEST 1: Geçersiz GenreId (<= 0) verildiğinde hata dönmeli
    // =========================================================================
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void WhenInvalidGenreIdIsGiven_Validator_ShouldBeReturnErrors(int genreId)
    {
        // ARRANGE
        var command = new DeleteGenreCommand(null!);
        command.GenreId = genreId;

        // ACT
        var validator = new DeleteGenreCommandValidator();
        var result = validator.Validate(command);

        // ASSERT
        result.Errors.Count.Should().BeGreaterThan(0);
    }

    // =========================================================================
    // TEST 2: Geçerli GenreId (> 0) verildiğinde hata dönmemeli
    // =========================================================================
    [Fact]
    public void WhenValidGenreIdIsGiven_Validator_ShouldNotBeReturnError()
    {
        // ARRANGE
        var command = new DeleteGenreCommand(null!);
        command.GenreId = 1;

        // ACT
        var validator = new DeleteGenreCommandValidator();
        var result = validator.Validate(command);

        // ASSERT
        result.Errors.Count.Should().Be(0);
    }
}
