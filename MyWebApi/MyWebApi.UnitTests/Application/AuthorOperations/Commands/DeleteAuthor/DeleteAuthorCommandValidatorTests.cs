using FluentAssertions;
using MyWebApi.Api.AuthorOperations.DeleteAuthor;
using MyWebApi.UnitTests.TestSetup;
using Xunit;

namespace MyWebApi.UnitTests.Application.AuthorOperations.Commands.DeleteAuthor;

public class DeleteAuthorCommandValidatorTests : IClassFixture<CommonTestFixture>
{
    // =========================================================================
    // TEST 1: Geçersiz AuthorId (<= 0) verildiğinde hata dönmeli
    // =========================================================================
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void WhenInvalidAuthorIdIsGiven_Validator_ShouldBeReturnErrors(int authorId)
    {
        // ARRANGE
        var command = new DeleteAuthorCommand(null!);
        command.AuthorId = authorId;

        // ACT
        var validator = new DeleteAuthorCommandValidator();
        var result = validator.Validate(command);

        // ASSERT
        result.Errors.Count.Should().BeGreaterThan(0);
    }

    // =========================================================================
    // TEST 2: Geçerli AuthorId (> 0) verildiğinde hata dönmemeli
    // =========================================================================
    [Fact]
    public void WhenValidAuthorIdIsGiven_Validator_ShouldNotBeReturnError()
    {
        // ARRANGE
        var command = new DeleteAuthorCommand(null!);
        command.AuthorId = 1;

        // ACT
        var validator = new DeleteAuthorCommandValidator();
        var result = validator.Validate(command);

        // ASSERT
        result.Errors.Count.Should().Be(0);
    }
}
