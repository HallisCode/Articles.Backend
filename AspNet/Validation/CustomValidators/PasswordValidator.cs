using FluentValidation;
using System.Linq;
using System.Text.RegularExpressions;

namespace AspNet.Validation.CustomValidators
{
	public static class PasswordValidator
	{
		private static Regex specialSymbol = new Regex(@"[~!@#$%^&*()_+=:;?><.,|/\\]");

		public static IRuleBuilderOptionsConditions<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
		{
			return ruleBuilder.Custom((field, context) =>
			{

				if (!field.Any(letter => char.IsUpper(letter)))
				{
					context.AddFailure("Должен содержать 1 заглавную букву.");
				}

				if (!field.Any(letter => char.IsDigit(letter)))
				{
					context.AddFailure("Должен содержать 1 цифру.");
				}

				if (!specialSymbol.IsMatch(field))
				{
					context.AddFailure("Должен содержать 1 спец. символ.");
				}

			});
		}
	}
}

