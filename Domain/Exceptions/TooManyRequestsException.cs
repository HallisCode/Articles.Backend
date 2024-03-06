namespace Domain.Exceptions
{
	public class TooManyRequestsException : HttpErrorBase
	{
		public TooManyRequestsException(string title, object? details = null) : base(title, details)
		{
		}

		protected override void Initialize()
		{
			StatusCode = 429;
		}
	}
}
