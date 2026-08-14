using FluentAssertions;
using MyWebApi.Api.BookOperations.GetBookDetail;
using MyWebApi.UnitTests.TestSetup;
using Xunit;

namespace MyWebApi.UnitTests.Application.BookOperations.Queries.GetBookDetail;

public class GetBookDetailQueryValidatorTests : IClassFixture<CommonTestFixture>
{
    // =========================================================================
    // TEST 1: Geçersiz BookId (<= 0) verildiğinde hata dönmeli
    // =========================================================================
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void WhenInvalidBookIdIsGiven_Validator_ShouldBeReturnErrors(int bookId)
    {
        // ARRANGE
        var query = new GetBookDetailQuery(null!, null!);
        query.BookId = bookId;

        // ACT
        var validator = new GetBookDetailQueryValidator();
        var result = validator.Validate(query);

        // ASSERT
        result.Errors.Count.Should().BeGreaterThan(0);
    }

    // =========================================================================
    // TEST 2: Geçerli BookId (> 0) verildiğinde hata dönmemeli
    // =========================================================================
    [Fact]
    public void WhenValidBookIdIsGiven_Validator_ShouldNotBeReturnError()
    {
        // ARRANGE
        var query = new GetBookDetailQuery(null!, null!);
        query.BookId = 1;

        // ACT
        var validator = new GetBookDetailQueryValidator();
        var result = validator.Validate(query);

        // ASSERT
        result.Errors.Count.Should().Be(0);
    }
}
