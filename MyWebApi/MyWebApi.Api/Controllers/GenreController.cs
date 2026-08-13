using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MyWebApi.Api.DBOperations;
using MyWebApi.Api.GenreOperations.CreateGenre;
using MyWebApi.Api.GenreOperations.DeleteGenre;
using MyWebApi.Api.GenreOperations.GetGenreDetail;
using MyWebApi.Api.GenreOperations.GetGenres;
using MyWebApi.Api.GenreOperations.UpdateGenre;

namespace MyWebApi.Api.Controllers;

[ApiController]
[Route("[controller]s")]
public class GenreController : ControllerBase
{
    private readonly BookStoreDbContext _context;
    private readonly IMapper _mapper;

    public GenreController(BookStoreDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetGenres()
    {
        GetGenresQuery query = new GetGenresQuery(_context, _mapper);
        var obj = query.Handle();
        return Ok(obj);
    }

    [HttpGet("{id}")]
    public IActionResult GetGenreDetail(int id)
    {
        GetGenreDetailQuery query = new GetGenreDetailQuery(_context, _mapper);
        query.GenreId = id;

        GetGenreDetailQueryValidator validator = new GetGenreDetailQueryValidator();
        validator.ValidateAndThrow(query);

        var obj = query.Handle();
        return Ok(obj);
    }

    [HttpPost]
    public IActionResult AddGenre([FromBody] CreateGenreModel newGenre)
    {
        CreateGenreCommand command = new CreateGenreCommand(_context);
        command.Model = newGenre;

        CreateGenreCommandValidator validator = new CreateGenreCommandValidator();
        validator.ValidateAndThrow(command);

        command.Handle();
        return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult UpdateGenre(int id, [FromBody] UpdateGenreModel updateGenre)
    {
        UpdateGenreCommand command = new UpdateGenreCommand(_context);
        command.GenreId = id;
        command.Model = updateGenre;

        UpdateGenreCommandValidator validator = new UpdateGenreCommandValidator();
        validator.ValidateAndThrow(command);

        command.Handle();
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteGenre(int id)
    {
        DeleteGenreCommand command = new DeleteGenreCommand(_context);
        command.GenreId = id;

        DeleteGenreCommandValidator validator = new DeleteGenreCommandValidator();
        validator.ValidateAndThrow(command);

        command.Handle();
        return Ok();
    }
}
