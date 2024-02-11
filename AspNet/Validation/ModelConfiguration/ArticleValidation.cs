using AspNet.Dto.Request;
using FluentValidation;

namespace AspNet.Validation.ModelConfiguration
{
    public class ArticleValidation : AbstractValidator<ArticleRequest>
    {
        public ArticleValidation()
        {
            RuleFor(article => article.Title).NotEmpty().MinimumLength(16).MaximumLength(128);

            RuleFor(article => article.Content).NotEmpty().MinimumLength(512);

            RuleFor(article => article.Tags).NotEmpty();
        }
    }
}
