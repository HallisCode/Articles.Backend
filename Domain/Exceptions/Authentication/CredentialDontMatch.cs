namespace Domain.Exceptions.Authentication
{
	public class CredentialDontMatch : HttpErrorBase
	{
		public CredentialDontMatch(string title, string? details = null) : base(title, details)
		{
		}

		protected override void Initialize()
		{
			StatusCode = 401;
		}
	}
}
