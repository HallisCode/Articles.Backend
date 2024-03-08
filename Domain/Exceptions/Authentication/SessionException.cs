namespace Domain.Exceptions.Authentication
{
	public class SessionException : HttpErrorBase
	{
		public SessionException(string title, object? details = null) : base(title, details)
		{
		}

		protected override void Initialize()
		{
			StatusCode = 401;
		}
	}
}
