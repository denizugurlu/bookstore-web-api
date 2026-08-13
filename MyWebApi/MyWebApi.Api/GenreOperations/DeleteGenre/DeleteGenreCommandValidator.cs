using FluentValidation;

namespace MyWebApi.Api.GenreOperations.DeleteGenre;

public class DeleteGenreCommandValidator : AbstractValidator<DeleteGenreCommand>
{
    public DeleteGenreCommandValidator()
    {
        RuleFor(command => command.GenreId).GreaterThan(0);
    }
}
