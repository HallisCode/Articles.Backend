namespace Domain.Exceptions
{
	public class ArgumentsMissingException : HttpErrorBase
	{
		public ArgumentsMissingException(string title, string? details = null) : base(title, details)
		{
		}

		protected override void Initialize()
		{
			StatusCode = 400;
		}
	}
}

