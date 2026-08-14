using AutoMapper;
using FluentAssertions;
using MyWebApi.Api.BookOperations.CreateBook;
using MyWebApi.Api.DBOperations;
using MyWebApi.Api.Entities;
using MyWebApi.UnitTests.TestSetup;
using Xunit;

namespace MyWebApi.UnitTests.Application.BookOperations.Commands.CreateBook;

public class CreateBookCommandTests : IClassFixture<CommonTestFixture>
{
    private readonly BookStoreDbContext _context;
    private readonly IMapper _mapper;

    // xUnit, IClassFixture sayesinde CommonTestFixture örneğini constructor'a otomatik enjekte eder
    public CreateBookCommandTests(CommonTestFixture testFixture)
    {
        _context = testFixture.Context;
        _mapper = testFixture.Mapper;
    }

    // =========================================================================
    // TEST 1: Veritabanında zaten kayıtlı olan bir kitap adı verildiğinde 
    // InvalidOperationException fırlatmalı ve doğru mesajı vermeli
    // =========================================================================
    [Fact]
    public void WhenAlreadyExistBookTitleIsGiven_InvalidOperationException_ShouldBeReturn()
    {
        // ------------------ 1. ARRANGE (Hazırlık) ------------------
        // Veritabanına test senaryosu için önceden bir kitap ekleyip kaydediyoruz:
        var book = new Book()
        {
            Title = "Test_WhenAlreadyExistBookTitleIsGiven",
            PageCount = 100,
            PublishDate = new DateTime(1990, 01, 01),
            GenreId = 1,
            AuthorId = 1
        };
        _context.Books.Add(book);
        _context.SaveChanges();

        // Şimdi eklediğimiz kitabın birebir aynı başlığına sahip yeni bir CreateBookCommand oluşturuyoruz:
        var command = new CreateBookCommand(_context, _mapper);
        command.Model = new CreateBookModel()
        {
            Title = book.Title
        };

        // ------------------ 2. ACT & 3. ASSERT (Çalıştırma ve Doğrulama) ------------------
        // FluentActions.Invoking: Bir metot hata fırlattığında bunu yakalayıp test edebilmemizi sağlar.
        FluentActions
            .Invoking(() => command.Handle())
            .Should()
            .Throw<InvalidOperationException>()                 // Beklenen hata türü
            .And
            .Message.Should().Be("Kitap zaten mevcut.");        // Beklenen hata mesajı
    }

    // =========================================================================
    // TEST 2: Geçerli girdiler verildiğinde kitap veritabanına başarıyla kaydedilmeli (Happy Path)
    // =========================================================================
    [Fact]
    public void WhenValidInputsAreGiven_Book_ShouldBeCreated()
    {
        // ------------------ 1. ARRANGE ------------------
        var command = new CreateBookCommand(_context, _mapper);
        var model = new CreateBookModel()
        {
            Title = "Hobbit",
            PageCount = 1000,
            PublishDate = DateTime.Now.Date.AddYears(-10),
            GenreId = 1,
            AuthorId = 1
        };
        command.Model = model;

        // ------------------ 2. ACT ------------------
        // command.Handle() metodunu tetikliyoruz (kitabın eklenmesi gerekiyor):
        FluentActions.Invoking(() => command.Handle()).Invoke();

        // ------------------ 3. ASSERT ------------------
        // Veritabanından "Hobbit" başlıklı kitabı çekiyoruz:
        var savedBook = _context.Books.SingleOrDefault(b => b.Title == model.Title);

        // Kitap veritabanında gerçekten var mı (null değil mi)?
        savedBook.Should().NotBeNull();

        // Veritabanındaki değerler, modele verdiğimiz değerlerle birebir eşleşiyor mu?
        savedBook!.PageCount.Should().Be(model.PageCount);
        savedBook.PublishDate.Should().Be(model.PublishDate);
        savedBook.GenreId.Should().Be(model.GenreId);
        savedBook.AuthorId.Should().Be(model.AuthorId);
    }
}
