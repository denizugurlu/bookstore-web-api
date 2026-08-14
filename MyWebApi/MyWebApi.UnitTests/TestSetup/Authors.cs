using MyWebApi.Api.DBOperations;
using MyWebApi.Api.Entities;

namespace MyWebApi.UnitTests.TestSetup;

public static class Authors
{
    public static void AddAuthors(this BookStoreDbContext context)
    {
        context.Authors.AddRange(
            new Author { Name = "Eric", Surname = "Ries", DateOfBirth = new DateTime(1978, 09, 22) },
            new Author { Name = "Frank", Surname = "Herbert", DateOfBirth = new DateTime(1920, 10, 08) },
            new Author { Name = "George", Surname = "Orwell", DateOfBirth = new DateTime(1903, 06, 25) }
        );
    }
}
