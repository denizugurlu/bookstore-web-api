using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyWebApi.Api.Common;
using MyWebApi.Api.DBOperations;

namespace MyWebApi.UnitTests.TestSetup;

public class CommonTestFixture
{
    public BookStoreDbContext Context { get; set; }

    public IMapper Mapper { get; set; }

    public CommonTestFixture()
    {
        // 1. RAM üzerinde çalışacak DbContext ayarlarını oluşturuyoruz
        // Guid.NewGuid() kullanarak her test çalıştırmasında izole, sıfır bir DB ismi veriyoruz
        var options = new DbContextOptionsBuilder<BookStoreDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Context = new BookStoreDbContext(options);

        // 2. Veritabanının oluşturulduğundan emin oluyoruz
        Context.Database.EnsureCreated();

        // 3. Yazdığımız yardımcı metotlarla test verilerini RAM veritabanına ekliyoruz
        Context.AddBooks();
        Context.AddGenres();
        Context.AddAuthors();
        Context.SaveChanges();

        // 4. Projemizdeki MappingProfile profilini AutoMapper'a yüklüyoruz
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        Mapper = mapperConfig.CreateMapper();
    }
}
