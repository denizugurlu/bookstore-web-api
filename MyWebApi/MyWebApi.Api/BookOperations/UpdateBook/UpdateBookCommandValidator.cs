using FluentValidation;

namespace MyWebApi.Api.BookOperations.UpdateBook;

public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookCommandValidator()
    {
        RuleFor(command => command.BookId).GreaterThan(0);
        RuleFor(command => command.Model.GenreId).GreaterThan(0).When(x => x.Model.GenreId != default);
        RuleFor(command => command.Model.AuthorId).GreaterThan(0).When(x => x.Model.AuthorId != default);
        RuleFor(command => command.Model.Title).NotEmpty().MinimumLength(4).When(x => !string.IsNullOrEmpty(x.Model.Title));
    }
}
