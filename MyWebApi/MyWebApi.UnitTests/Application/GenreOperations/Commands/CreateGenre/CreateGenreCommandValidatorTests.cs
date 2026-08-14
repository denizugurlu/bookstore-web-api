using FluentAssertions;
using MyWebApi.Api.GenreOperations.CreateGenre;
using MyWebApi.UnitTests.TestSetup;
using Xunit;

namespace MyWebApi.UnitTests.Application.GenreOperations.Commands.CreateGenre;

public class CreateGenreCommandValidatorTests : IClassFixture<CommonTestFixture>
{
    // =========================================================================
    // TEST 1: Geçersiz isimler (boş veya < 4 karakter) verildiğinde hata dönmeli
    // =========================================================================
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("a")]
    [InlineData("ab")]
    [InlineData("abc")] // 3 karakter (Minimum 4 karakter olmalı)
    public void WhenInvalidGenreNameIsGiven_Validator_ShouldBeReturnErrors(string name)
    {
        // ARRANGE
        var command = new CreateGenreCommand(null!);
        command.Model = new CreateGenreModel { Name = name };

        // ACT
        var validator = new CreateGenreCommandValidator();
        var result = validator.Validate(command);

        // ASSERT
        result.Errors.Count.Should().BeGreaterThan(0);
    }

    // =========================================================================
    // TEST 2: Geçerli isim (>= 4 karakter) verildiğinde hata dönmemeli
    // =========================================================================
    [Theory]
    [InlineData("Romance")]
    [InlineData("Action")]
    [InlineData("History")]
    public void WhenValidGenreNameIsGiven_Validator_ShouldNotBeReturnError(string name)
    {
        // ARRANGE
        var command = new CreateGenreCommand(null!);
        command.Model = new CreateGenreModel { Name = name };

        // ACT
        var validator = new CreateGenreCommandValidator();
        var result = validator.Validate(command);

        // ASSERT
        result.Errors.Count.Should().Be(0);
    }
}
