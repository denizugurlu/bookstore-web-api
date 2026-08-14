using AutoMapper;
using FluentAssertions;
using MyWebApi.Api.AuthorOperations.GetAuthorDetail;
using MyWebApi.Api.DBOperations;
using MyWebApi.Api.Entities;
using MyWebApi.UnitTests.TestSetup;
using Xunit;

namespace MyWebApi.UnitTests.Application.AuthorOperations.Queries.GetAuthorDetail;

public class GetAuthorDetailQueryTests : IClassFixture<CommonTestFixture>
{
    private readonly BookStoreDbContext _context;
    private readonly IMapper _mapper;

    public GetAuthorDetailQueryTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
        _mapper = testFixture.Mapper;
    }

    // =========================================================================
    // TEST 1: Olmayan bir yazar arandığında InvalidOperationException fırlatmalı
    // =========================================================================
    [Fact]
    public void WhenAuthorIsNotFound_InvalidOperationException_ShouldBeReturn()
    {
        // ARRANGE
        var query = new GetAuthorDetailQuery(_context, _mapper);
        query.AuthorId = 99999;

        // ACT & ASSERT
        FluentActions
            .Invoking(() => query.Handle())
            .Should()
            .Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("Yazar bulunamadı.");
    }

    // =========================================================================
    // TEST 2: Var olan yazar arandığında doğru ViewModel dönmeli
    // =========================================================================
    [Fact]
    public void WhenValidAuthorIdIsGiven_AuthorDetail_ShouldBeReturned()
    {
        // ARRANGE
        var author = new Author
        {
            Name = "Fyodor",
            Surname = "Dostoyevsky",
            DateOfBirth = new DateTime(1821, 11, 11)
        };
        _context.Authors.Add(author);
        _context.SaveChanges();

        var query = new GetAuthorDetailQuery(_context, _mapper);
        query.AuthorId = author.Id;

        // ACT
        var result = query.Handle();

        // ASSERT
        result.Should().NotBeNull();
        result.Id.Should().Be(author.Id);
        result.Name.Should().Be(author.Name);
        result.Surname.Should().Be(author.Surname);
        result.DateOfBirth.Should().Be(author.DateOfBirth.Date.ToString("dd/MM/yyyy"));
    }
}
