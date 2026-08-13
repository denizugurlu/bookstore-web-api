using FluentValidation;

namespace MyWebApi.Api.GenreOperations.GetGenreDetail;

public class GetGenreDetailQueryValidator : AbstractValidator<GetGenreDetailQuery>
{
    public GetGenreDetailQueryValidator()
    {
        RuleFor(query => query.GenreId).GreaterThan(0);
    }
}
