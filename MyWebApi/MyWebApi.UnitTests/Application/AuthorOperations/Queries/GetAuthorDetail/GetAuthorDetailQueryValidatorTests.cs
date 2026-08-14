using FluentAssertions;
using MyWebApi.Api.AuthorOperations.GetAuthorDetail;
using MyWebApi.UnitTests.TestSetup;
using Xunit;

namespace MyWebApi.UnitTests.Application.AuthorOperations.Queries.GetAuthorDetail;

public class GetAuthorDetailQueryValidatorTests : IClassFixture<CommonTestFixture>
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
        var query = new GetAuthorDetailQuery(null!, null!);
        query.AuthorId = authorId;

        // ACT
        var validator = new GetAuthorDetailQueryValidator();
        var result = validator.Validate(query);

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
        var query = new GetAuthorDetailQuery(null!, null!);
        query.AuthorId = 1;

        // ACT
        var validator = new GetAuthorDetailQueryValidator();
        var result = validator.Validate(query);

        // ASSERT
        result.Errors.Count.Should().Be(0);
    }
}
