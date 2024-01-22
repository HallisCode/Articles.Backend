namespace Domain.Exceptions.Authorization
{
	public class AccesDenied : IntentionalInternalException
	{
		public AccesDenied(string title, string? details = null) : base(title, details)
		{
		}

		protected override void Initialize()
		{
			this.StatusCode = 403;
		}
	}
}
