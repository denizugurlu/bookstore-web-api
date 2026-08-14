using FluentAssertions;
using MyWebApi.Api.BookOperations.DeleteBook;
using MyWebApi.UnitTests.TestSetup;
using Xunit;
namespace MyWebApi.UnitTests.Application.BookOperations.Commands.DeleteBook;

public class DeleteBookCommandValidatorTests : IClassFixture<CommonTestFixture>
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void WhenInvalidBookIdIsGiven_Validator_ShouldBeReturnErrors(int bookId)
    {
        // 1. ARRANGE
        var command = new DeleteBookCommand(null!);
        command.BookId = bookId;

        // 2. ACT
        var validator = new DeleteBookCommandValidator();
        var result = validator.Validate(command);

        // 3. ASSERT
        result.Errors.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public void WhenValidBookIdIsGiven_Validator_ShouldNotBeReturnError()
    {
        var command = new DeleteBookCommand(null!);
        command.BookId = 1;

        var validator = new DeleteBookCommandValidator();
        var result = validator.Validate(command);

        result.Errors.Count.Should().Be(0);

    }


}