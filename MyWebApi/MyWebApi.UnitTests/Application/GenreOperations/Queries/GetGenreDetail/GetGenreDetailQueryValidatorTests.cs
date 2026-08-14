using FluentAssertions;
using MyWebApi.Api.GenreOperations.GetGenreDetail;
using MyWebApi.UnitTests.TestSetup;
using Xunit;

namespace MyWebApi.UnitTests.Application.GenreOperations.Queries.GetGenreDetail;

public class GetGenreDetailQueryValidatorTests : IClassFixture<CommonTestFixture>
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
        var query = new GetGenreDetailQuery(null!, null!);
        query.GenreId = genreId;

        // ACT
        var validator = new GetGenreDetailQueryValidator();
        var result = validator.Validate(query);

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
        var query = new GetGenreDetailQuery(null!, null!);
        query.GenreId = 1;

        // ACT
        var validator = new GetGenreDetailQueryValidator();
        var result = validator.Validate(query);

        // ASSERT
        result.Errors.Count.Should().Be(0);
    }
}
