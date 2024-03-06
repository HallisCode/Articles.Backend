namespace Domain.Exceptions
{
	public class ValidationException : HttpErrorBase
	{
		public ValidationException(string title, object? details = null) : base(title, details)
		{
		}

		protected override void Initialize()
		{
			this.StatusCode = 400;
		}
	}
}
