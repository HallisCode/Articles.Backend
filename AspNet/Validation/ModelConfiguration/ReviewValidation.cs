using AspNet.Dto.Request;
using FluentValidation;

namespace AspNet.Validation.ModelConfiguration
{
    public class ReviewValidation : AbstractValidator<ReviewRequest>
    {
        public ReviewValidation()
        {
            RuleFor(review => review.ArticleId).NotEmpty();

            RuleFor(review => review.Content).NotEmpty().MinimumLength(256);

            RuleFor(review => review.Type).NotNull().IsInEnum();
        }
    }
}
