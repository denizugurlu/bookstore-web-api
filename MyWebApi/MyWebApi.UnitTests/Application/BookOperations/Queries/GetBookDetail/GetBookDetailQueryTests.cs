using AutoMapper;
using FluentAssertions;
using MyWebApi.Api.BookOperations.GetBookDetail;
using MyWebApi.Api.DBOperations;
using MyWebApi.Api.Entities;
using MyWebApi.UnitTests.TestSetup;
using Xunit;

namespace MyWebApi.UnitTests.Application.BookOperations.Queries.GetBookDetail;

public class GetBookDetailQueryTests : IClassFixture<CommonTestFixture>
{
    private readonly BookStoreDbContext _context;
    private readonly IMapper _mapper;

    public GetBookDetailQueryTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
        _mapper = testFixture.Mapper;
    }

    // =========================================================================
    // TEST 1: Olmayan bir BookId arandığında InvalidOperationException fırlatmalı
    // =========================================================================
    [Fact]
    public void WhenBookIsNotFound_InvalidOperationException_ShouldBeReturn()
    {
        // ARRANGE
        var query = new GetBookDetailQuery(_context, _mapper);
        query.BookId = 99999;

        // ACT & ASSERT
        FluentActions
            .Invoking(() => query.Handle())
            .Should()
            .Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("Kitap bulunamadı!");
    }

    // =========================================================================
    // TEST 2: Var olan kitap arandığında doğru detay ViewModel'i dönmeli
    // =========================================================================
    [Fact]
    public void WhenValidBookIdIsGiven_BookDetail_ShouldBeReturned()
    {
        // ARRANGE: İlişkili Genre ve Author ile birlikte kitap ekliyoruz
        var book = new Book
        {
            Title = "Test_GetBookDetail",
            PageCount = 350,
            PublishDate = new DateTime(2010, 05, 20),
            GenreId = 1,
            AuthorId = 1
        };
        _context.Books.Add(book);
        _context.SaveChanges();

        var query = new GetBookDetailQuery(_context, _mapper);
        query.BookId = book.Id;

        // ACT
        var result = query.Handle();

        // ASSERT
        result.Should().NotBeNull();
        result.Title.Should().Be(book.Title);
        result.PageCount.Should().Be(book.PageCount);
        result.PublishDate.Should().Be(book.PublishDate.Date.ToString("dd/MM/yyyy"));
    }
}
