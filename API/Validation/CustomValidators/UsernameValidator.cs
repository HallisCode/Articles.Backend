using FluentValidation;
using System.Text.RegularExpressions;

namespace AspNet.Validation.CustomValidators
{
	public static class UsernameValidator
	{
		private static Regex allowedSymbols = new Regex("^[a-zA-Z0-9_]+$");

		private static Regex duplicateUnderscore = new Regex("^(?!.*__).*$");

		public static IRuleBuilderOptionsConditions<T, string> Username<T>(this IRuleBuilder<T, string> ruleBuilder)
		{
			return ruleBuilder.Custom((field, context) =>
			{

				if (!char.IsLetter(field[0]))
				{
					context.AddFailure("Должен начинаться с буквы.");
				}

				if (!allowedSymbols.IsMatch(field))
				{
					context.AddFailure("Может содержать только латинские буквы, цифры и знак нижнего подчеркивания.");
				}

				if (!duplicateUnderscore.IsMatch(field))
				{
					context.AddFailure("Знак нижнего подчеркивания не должен идти более одного раза подряд.");
				}
			});
		}
	}
}
