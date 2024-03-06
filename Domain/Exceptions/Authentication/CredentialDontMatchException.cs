namespace Domain.Exceptions.Authentication
{
	public class CredentialDontMatchException : HttpErrorBase
	{
		public CredentialDontMatchException(string title, object? details = null) : base(title, details)
		{
		}

		protected override void Initialize()
		{
			StatusCode = 401;
		}
	}
}
