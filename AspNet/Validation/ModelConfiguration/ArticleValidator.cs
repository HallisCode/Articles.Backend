using AspNet.Dto.Request;
using FluentValidation;

namespace AspNet.Validation.ModelConfiguration
{
	public class ArticleValidator : AbstractValidator<ArticleRequest>
	{
		public ArticleValidator()
		{
			RuleFor(article => article.Title).MinimumLength(16).MaximumLength(128);

			RuleFor(article => article.Content).MinimumLength(1024);

			RuleFor(article => article.Tags);
		}
	}
}
