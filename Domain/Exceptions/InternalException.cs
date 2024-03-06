namespace Domain.Exceptions
{
	public class InternalException : HttpErrorBase
	{
		public InternalException(string title, object? details = null) : base(title, details)
		{
		}

		protected override void Initialize()
		{
			this.StatusCode = 500;
		}
	}
}
