using AutoMapper;
using FluentAssertions;
using MyWebApi.Api.DBOperations;
using MyWebApi.Api.Entities;
using MyWebApi.Api.GenreOperations.GetGenreDetail;
using MyWebApi.UnitTests.TestSetup;
using Xunit;

namespace MyWebApi.UnitTests.Application.GenreOperations.Queries.GetGenreDetail;

public class GetGenreDetailQueryTests : IClassFixture<CommonTestFixture>
{
    private readonly BookStoreDbContext _context;
    private readonly IMapper _mapper;

    public GetGenreDetailQueryTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
        _mapper = testFixture.Mapper;
    }

    // =========================================================================
    // TEST 1: Olmayan veya Pasif (IsActive = false) olan Genre sorgulandığında hata fırlatmalı
    // =========================================================================
    [Fact]
    public void WhenGenreIsNotFound_InvalidOperationException_ShouldBeReturn()
    {
        // ARRANGE: Pasif bir tür ekliyoruz
        var passiveGenre = new Genre { Name = "Passive Genre", IsActive = false };
        _context.Genres.Add(passiveGenre);
        _context.SaveChanges();

        var query = new GetGenreDetailQuery(_context, _mapper);
        query.GenreId = passiveGenre.Id; // Pasif olduğu için bulunamadı demeli

        // ACT & ASSERT
        FluentActions
            .Invoking(() => query.Handle())
            .Should()
            .Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("Kitap türü bulunamadı.");
    }

    // =========================================================================
    // TEST 2: Var olan aktif Genre sorgulandığında detay dönmeli
    // =========================================================================
    [Fact]
    public void WhenValidGenreIdIsGiven_GenreDetail_ShouldBeReturned()
    {
        // ARRANGE
        var genre = new Genre { Name = "Psychology", IsActive = true };
        _context.Genres.Add(genre);
        _context.SaveChanges();

        var query = new GetGenreDetailQuery(_context, _mapper);
        query.GenreId = genre.Id;

        // ACT
        var result = query.Handle();

        // ASSERT
        result.Should().NotBeNull();
        result.Id.Should().Be(genre.Id);
        result.Name.Should().Be(genre.Name);
    }
}
