using AspNet.Dto.Request;
using FluentValidation;

namespace AspNet.Validation.ModelConfiguration
{
    public class ReviewValidator : AbstractValidator<ReviewRequest>
    {
        public ReviewValidator()
        {
            RuleFor(review => review.ArticleId).NotEmpty();

            RuleFor(review => review.Content).NotEmpty().MinimumLength(256);

            RuleFor(review => review.Type).NotNull().IsInEnum();
        }
    }
}
