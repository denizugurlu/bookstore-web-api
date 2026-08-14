using AutoMapper;
using FluentAssertions;
using MyWebApi.Api.AuthorOperations.CreateAuthor;
using MyWebApi.Api.DBOperations;
using MyWebApi.Api.Entities;
using MyWebApi.UnitTests.TestSetup;
using Xunit;

namespace MyWebApi.UnitTests.Application.AuthorOperations.Commands.CreateAuthor;

public class CreateAuthorCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly BookStoreDbContext _context;
    private readonly IMapper _mapper;

    public CreateAuthorCommandTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
        _mapper = testFixture.Mapper;
    }

    // =========================================================================
    // TEST 1: Zaten var olan isim ve soyisimde yazar eklenmeye çalışıldığında hata vermeli
    // =========================================================================
    [Fact]
    public void WhenAlreadyExistAuthorIsGiven_InvalidOperationException_ShouldBeReturn()
    {
        // ARRANGE: DB'ye yazar ekliyoruz
        var author = new Author
        {
            Name = "Test_Author_Name",
            Surname = "Test_Author_Surname",
            DateOfBirth = new DateTime(1975, 1, 1)
        };
        _context.Authors.Add(author);
        _context.SaveChanges();

        var command = new CreateAuthorCommand(_context, _mapper);
        command.Model = new CreateAuthorModel
        {
            Name = author.Name,
            Surname = author.Surname,
            DateOfBirth = author.DateOfBirth
        };

        // ACT & ASSERT
        FluentActions
            .Invoking(() => command.Handle())
            .Should()
            .Throw<InvalidOperationException>()
            .And
            .Message.Should().Be("Yazar zaten mevcut.");
    }

    // =========================================================================
    // TEST 2: Geçerli yazar bilgisi verildiğinde yazar başarıyla eklenmeli
    // =========================================================================
    [Fact]
    public void WhenValidInputsAreGiven_Author_ShouldBeCreated()
    {
        // ARRANGE
        var command = new CreateAuthorCommand(_context, _mapper);
        var model = new CreateAuthorModel
        {
            Name = "J.K.",
            Surname = "Rowling",
            DateOfBirth = new DateTime(1965, 07, 31)
        };
        command.Model = model;

        // ACT
        FluentActions.Invoking(() => command.Handle()).Invoke();

        // ASSERT
        var savedAuthor = _context.Authors.SingleOrDefault(x => x.Name == model.Name && x.Surname == model.Surname);
        savedAuthor.Should().NotBeNull();
        savedAuthor!.DateOfBirth.Should().Be(model.DateOfBirth);
    }
}
