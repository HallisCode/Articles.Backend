namespace Domain.Exceptions.Authentication
{
	public class JWTException : HttpErrorBase
	{
		public JWTException(string title, object? details = null) : base(title, details)
		{
		}

		protected override void Initialize()
		{
			StatusCode = 401;
		}
	}
}

