using Microsoft.AspNetCore.Mvc;
using MyWebApi.Api.BookOperations.CreateBook;
using MyWebApi.Api.BookOperations.DeleteBook;
using MyWebApi.Api.BookOperations.GetBookDetail;
using MyWebApi.Api.BookOperations.GetBooks;
using MyWebApi.Api.BookOperations.UpdateBook;
using MyWebApi.Api.DBOperations;

namespace MyWebApi.Api.Controllers;

[ApiController]
[Route("api/[controller]s")] // api/books
public class BookController : ControllerBase
{
    private readonly BookStoreDbContext _context;

    public BookController(BookStoreDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetBooks()
    {
        GetBooksQuery query = new GetBooksQuery(_context);
        var result = query.Handle();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        GetBookDetailQuery query = new GetBookDetailQuery(_context);
        query.BookId = id;
        var result = query.Handle();
        return Ok(result);
    }

    [HttpPost]
    public IActionResult AddBook([FromBody] CreateBookModel newBook)
    {
        CreateBookCommand command = new CreateBookCommand(_context);
        command.Model = newBook;
        command.Handle();
        return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult UpdateBook(int id, [FromBody] UpdateBookModel updatedBook)
    {
        UpdateBookCommand command = new UpdateBookCommand(_context);
        command.BookId = id;
        command.Model = updatedBook;
        command.Handle();
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBook(int id)
    {
        DeleteBookCommand command = new DeleteBookCommand(_context);
        command.BookId = id;
        command.Handle();
        return Ok();
    }
}
