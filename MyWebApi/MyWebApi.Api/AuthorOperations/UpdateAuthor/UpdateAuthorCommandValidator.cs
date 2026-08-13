using FluentValidation;

namespace MyWebApi.Api.AuthorOperations.UpdateAuthor;

public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
{
    public UpdateAuthorCommandValidator()
    {
        RuleFor(command => command.AuthorId).GreaterThan(0);
        RuleFor(command => command.Model.Name).MinimumLength(2).When(x => !string.IsNullOrEmpty(x.Model.Name.Trim()));
        RuleFor(command => command.Model.Surname).MinimumLength(2).When(x => !string.IsNullOrEmpty(x.Model.Surname.Trim()));
        RuleFor(command => command.Model.DateOfBirth.Date).LessThan(DateTime.Now.Date).When(x => x.Model.DateOfBirth != default);
    }
}
