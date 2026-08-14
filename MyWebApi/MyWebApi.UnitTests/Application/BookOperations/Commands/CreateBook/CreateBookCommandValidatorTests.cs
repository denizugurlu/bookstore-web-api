using FluentAssertions;
using MyWebApi.Api.BookOperations.CreateBook;
using MyWebApi.UnitTests.TestSetup;
using Xunit;

namespace MyWebApi.UnitTests.Application.BookOperations.Commands.CreateBook;

// IClassFixture<CommonTestFixture>: Test sınıfının CommonTestFixture ayarlarını kullanmasını sağlar
public class CreateBookCommandValidatorTests : IClassFixture<CommonTestFixture>
{
    // =========================================================================
    // TEST 1: Geçersiz veriler girildiğinde validator hata üretmeli mi?
    // [Fact]: Tek bir senaryoyu test eden xUnit niteliğidir (Parametre almaz).
    // =========================================================================
    [Fact]
    public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnErrors()
    {
        // ------------------ 1. ARRANGE (Hazırlık) ------------------
        // Sadece doğrulama (validation) kurallarını test edeceğimiz için DbContext ve Mapper'a gerek yok (null verebiliriz).
        var command = new CreateBookCommand(null!, null!);
        
        // Hatalı alanlarla dolu bir model hazırlıyoruz:
        command.Model = new CreateBookModel()
        {
            Title = "",                     // HATA: Title boş olamaz, en az 4 karakter olmalı
            PageCount = 0,                  // HATA: PageCount 0'dan büyük olmalı
            PublishDate = DateTime.Now.Date,// HATA: Yayın tarihi bugünden önce (geçmişte) olmalı
            GenreId = 0,                    // HATA: GenreId 0'dan büyük olmalı
            AuthorId = 0                    // HATA: AuthorId 0'dan büyük olmalı
        };

        // ------------------ 2. ACT (Çalıştırma) ------------------
        // Doğrulayıcımızı oluşturuyoruz ve hazırladığımız komutu sınıyoruz
        var validator = new CreateBookCommandValidator();
        var result = validator.Validate(command);

        // ------------------ 3. ASSERT (Doğrulama) ------------------
        // FluentAssertions kullanarak hata sayısının 0'dan büyük olduğunu kontrol ediyoruz
        result.Errors.Count.Should().BeGreaterThan(0);
    }

    // =========================================================================
    // TEST 2: Yayın tarihi bugüne eşit veya ileri bir tarih girildiğinde hata dönmeli
    // =========================================================================
    [Fact]
    public void WhenDateTimeEqualNowIsGiven_Validator_ShouldBeReturnError()
    {
        // ------------------ 1. ARRANGE ------------------
        // Diğer tüm alanlar kurallara uygun ama sadece PublishDate bugünün tarihi (hatalı)
        var command = new CreateBookCommand(null!, null!);
        command.Model = new CreateBookModel()
        {
            Title = "Lord Of The Rings",
            PageCount = 100,
            PublishDate = DateTime.Now.Date, // HATA: Kuralımız LessThan(DateTime.Now.Date) idi
            GenreId = 1,
            AuthorId = 1
        };

        // ------------------ 2. ACT ------------------
        var validator = new CreateBookCommandValidator();
        var result = validator.Validate(command);

        // ------------------ 3. ASSERT ------------------
        // En az 1 doğrulama hatası üretilmiş olmalı
        result.Errors.Count.Should().BeGreaterThan(0);
    }

    // =========================================================================
    // TEST 3: Birden fazla hatalı kombinasyonu tek bir metotla test etme
    // [Theory]: Parametreli testtir. Aynı test mantığını farklı girdi değerleriyle tekrar tekrar çalıştırır.
    // [InlineData]: Her satır, teste gönderilecek bir veri kümesini temsil eder.
    // =========================================================================
    [Theory]
    [InlineData("Lord Of The Rings", 0, 0, 0)]      // PageCount, GenreId, AuthorId 0
    [InlineData("Lord Of The Rings", 100, 0, 0)]    // GenreId, AuthorId 0
    [InlineData("Lord Of The Rings", 0, 1, 0)]      // PageCount, AuthorId 0
    [InlineData("Lor", 100, 1, 1)]                  // Title 3 karakter (Kural: minimum 4 karakter olmalı)
    [InlineData("", 100, 1, 1)]                     // Title boş
    [InlineData(" ", 100, 1, 1)]                    // Title sadece boşluk
    public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnErrors_WithTheory(
        string title, int pageCount, int genreId, int authorId)
    {
        // ------------------ 1. ARRANGE ------------------
        var command = new CreateBookCommand(null!, null!);
        command.Model = new CreateBookModel()
        {
            Title = title,
            PageCount = pageCount,
            PublishDate = DateTime.Now.Date.AddYears(-1), // Geçerli geçmiş bir tarih
            GenreId = genreId,
            AuthorId = authorId
        };

        // ------------------ 2. ACT ------------------
        var validator = new CreateBookCommandValidator();
        var result = validator.Validate(command);

        // ------------------ 3. ASSERT ------------------
        // Gönderilen her senaryoda en az bir hata dönmeli
        result.Errors.Count.Should().BeGreaterThan(0);
    }

    // =========================================================================
    // TEST 4: Bütün alanlar geçerli girildiğinde HİÇBİR hata dönmemeli (Happy Path)
    // =========================================================================
    [Fact]
    public void WhenValidInputsAreGiven_Validator_ShouldNotBeReturnError()
    {
        // ------------------ 1. ARRANGE ------------------
        // Tüm kurallara eksiksiz uyan geçerli bir model:
        var command = new CreateBookCommand(null!, null!);
        command.Model = new CreateBookModel()
        {
            Title = "Lord Of The Rings",
            PageCount = 100,
            PublishDate = DateTime.Now.Date.AddYears(-2), // 2 yıl önce (geçerli)
            GenreId = 1,
            AuthorId = 1
        };

        // ------------------ 2. ACT ------------------
        var validator = new CreateBookCommandValidator();
        var result = validator.Validate(command);

        // ------------------ 3. ASSERT ------------------
        // Hata sayısı tam olarak 0 olmalı!
        result.Errors.Count.Should().Be(0);
    }
}
