namespace Domain.Exceptions.Authentication
{
	public class SessionException : IntentionalInternalException
	{
		public SessionException(string title, string? details = null) : base(title, details)
		{
		}

		protected override void Initialize()
		{
			StatusCode = 401;
		}
	}
}
