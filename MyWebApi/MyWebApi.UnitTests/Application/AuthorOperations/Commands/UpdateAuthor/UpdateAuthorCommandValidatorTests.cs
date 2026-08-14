using FluentAssertions;
using MyWebApi.Api.AuthorOperations.UpdateAuthor;
using MyWebApi.UnitTests.TestSetup;
using Xunit;

namespace MyWebApi.UnitTests.Application.AuthorOperations.Commands.UpdateAuthor;

public class UpdateAuthorCommandValidatorTests : IClassFixture<CommonTestFixture>
{
    // =========================================================================
    // TEST 1: Geçersiz AuthorId, kısa isim veya ileri tarih verildiğinde hata dönmeli
    // =========================================================================
    [Theory]
    [InlineData(0, "George", "Orwell")]   // AuthorId 0 olamaz
    [InlineData(-1, "George", "Orwell")]  // AuthorId negatif olamaz
    [InlineData(1, "G", "Orwell")]        // Name doluysa min 2 karakter olmalı
    [InlineData(1, "George", "O")]        // Surname doluysa min 2 karakter olmalı
    public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnErrors(int authorId, string name, string surname)
    {
        // ARRANGE
        var command = new UpdateAuthorCommand(null!);
        command.AuthorId = authorId;
        command.Model = new UpdateAuthorModel
        {
            Name = name,
            Surname = surname,
            DateOfBirth = new DateTime(1980, 1, 1)
        };

        // ACT
        var validator = new UpdateAuthorCommandValidator();
        var result = validator.Validate(command);

        // ASSERT
        result.Errors.Count.Should().BeGreaterThan(0);
    }

    // =========================================================================
    // TEST 2: Doğum tarihi gelecek bir tarih verilirse hata dönmeli
    // =========================================================================
    [Fact]
    public void WhenDateOfBirthIsFutureDate_Validator_ShouldBeReturnError()
    {
        // ARRANGE
        var command = new UpdateAuthorCommand(null!);
        command.AuthorId = 1;
        command.Model = new UpdateAuthorModel
        {
            Name = "George",
            Surname = "Orwell",
            DateOfBirth = DateTime.Now.Date.AddDays(1) // İleri tarih (Geçersiz)
        };

        // ACT
        var validator = new UpdateAuthorCommandValidator();
        var result = validator.Validate(command);

        // ASSERT
        result.Errors.Count.Should().BeGreaterThan(0);
    }

    // =========================================================================
    // TEST 3: Geçerli girdiler verildiğinde hata dönmemeli
    // =========================================================================
    [Theory]
    [InlineData(1, "George", "Orwell")]
    [InlineData(1, "", "")] // Boş bırakılan alanlar güncellenmez, validator geçer
    public void WhenValidInputsAreGiven_Validator_ShouldNotBeReturnError(int authorId, string name, string surname)
    {
        // ARRANGE
        var command = new UpdateAuthorCommand(null!);
        command.AuthorId = authorId;
        command.Model = new UpdateAuthorModel
        {
            Name = name,
            Surname = surname,
            DateOfBirth = default
        };

        // ACT
        var validator = new UpdateAuthorCommandValidator();
        var result = validator.Validate(command);

        // ASSERT
        result.Errors.Count.Should().Be(0);
    }
}
