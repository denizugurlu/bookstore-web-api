using FluentAssertions;
using MyWebApi.Api.AuthorOperations.CreateAuthor;
using MyWebApi.UnitTests.TestSetup;
using Xunit;

namespace MyWebApi.UnitTests.Application.AuthorOperations.Commands.CreateAuthor;

public class CreateAuthorCommandValidatorTests : IClassFixture<CommonTestFixture>
{
    // =========================================================================
    // TEST 1: Geçersiz isim, soyisim veya gelecek tarih verildiğinde hata dönmeli
    // =========================================================================
    [Theory]
    [InlineData("", "Orwell")]      // Name boş
    [InlineData("G", "Orwell")]     // Name 1 karakter (minimum 2 olmalı)
    [InlineData("George", "")]      // Surname boş
    [InlineData("George", "O")]     // Surname 1 karakter (minimum 2 olmalı)
    public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnErrors(string name, string surname)
    {
        // ARRANGE
        var command = new CreateAuthorCommand(null!, null!);
        command.Model = new CreateAuthorModel
        {
            Name = name,
            Surname = surname,
            DateOfBirth = new DateTime(1980, 01, 01)
        };

        // ACT
        var validator = new CreateAuthorCommandValidator();
        var result = validator.Validate(command);

        // ASSERT
        result.Errors.Count.Should().BeGreaterThan(0);
    }

    // =========================================================================
    // TEST 2: Doğum tarihi bugüne eşit veya ileri bir tarihse hata dönmeli
    // =========================================================================
    [Fact]
    public void WhenDateOfBirthIsFutureDate_Validator_ShouldBeReturnError()
    {
        // ARRANGE
        var command = new CreateAuthorCommand(null!, null!);
        command.Model = new CreateAuthorModel
        {
            Name = "George",
            Surname = "Orwell",
            DateOfBirth = DateTime.Now.Date.AddDays(1) // İleri tarih (Geçersiz)
        };

        // ACT
        var validator = new CreateAuthorCommandValidator();
        var result = validator.Validate(command);

        // ASSERT
        result.Errors.Count.Should().BeGreaterThan(0);
    }

    // =========================================================================
    // TEST 3: Geçerli girdiler verildiğinde hata dönmemeli
    // =========================================================================
    [Fact]
    public void WhenValidInputsAreGiven_Validator_ShouldNotBeReturnError()
    {
        // ARRANGE
        var command = new CreateAuthorCommand(null!, null!);
        command.Model = new CreateAuthorModel
        {
            Name = "George",
            Surname = "Orwell",
            DateOfBirth = new DateTime(1903, 06, 25)
        };

        // ACT
        var validator = new CreateAuthorCommandValidator();
        var result = validator.Validate(command);

        // ASSERT
        result.Errors.Count.Should().Be(0);
    }
}
