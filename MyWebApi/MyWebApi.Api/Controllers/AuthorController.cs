using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MyWebApi.Api.AuthorOperations.CreateAuthor;
using MyWebApi.Api.AuthorOperations.DeleteAuthor;
using MyWebApi.Api.AuthorOperations.GetAuthorDetail;
using MyWebApi.Api.AuthorOperations.GetAuthors;
using MyWebApi.Api.AuthorOperations.UpdateAuthor;
using MyWebApi.Api.DBOperations;

namespace MyWebApi.Api.Controllers;

[ApiController]
[Route("[controller]s")]
public class AuthorController : ControllerBase
{
    private readonly BookStoreDbContext _context;
    private readonly IMapper _mapper;

    public AuthorController(BookStoreDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetAuthors()
    {
        GetAuthorsQuery query = new GetAuthorsQuery(_context, _mapper);
        var obj = query.Handle();
        return Ok(obj);
    }

    [HttpGet("{id}")]
    public IActionResult GetAuthorDetail(int id)
    {
        GetAuthorDetailQuery query = new GetAuthorDetailQuery(_context, _mapper);
        query.AuthorId = id;

        GetAuthorDetailQueryValidator validator = new GetAuthorDetailQueryValidator();
        validator.ValidateAndThrow(query);

        var obj = query.Handle();
        return Ok(obj);
    }

    [HttpPost]
    public IActionResult AddAuthor([FromBody] CreateAuthorModel newAuthor)
    {
        CreateAuthorCommand command = new CreateAuthorCommand(_context, _mapper);
        command.Model = newAuthor;

        CreateAuthorCommandValidator validator = new CreateAuthorCommandValidator();
        validator.ValidateAndThrow(command);

        command.Handle();
        return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult UpdateAuthor(int id, [FromBody] UpdateAuthorModel updateAuthor)
    {
        UpdateAuthorCommand command = new UpdateAuthorCommand(_context);
        command.AuthorId = id;
        command.Model = updateAuthor;

        UpdateAuthorCommandValidator validator = new UpdateAuthorCommandValidator();
        validator.ValidateAndThrow(command);

        command.Handle();
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteAuthor(int id)
    {
        DeleteAuthorCommand command = new DeleteAuthorCommand(_context);
        command.AuthorId = id;

        DeleteAuthorCommandValidator validator = new DeleteAuthorCommandValidator();
        validator.ValidateAndThrow(command);

        command.Handle();
        return Ok();
    }
}
