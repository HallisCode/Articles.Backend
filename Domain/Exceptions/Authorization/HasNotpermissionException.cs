namespace Domain.Exceptions.Authorization
{
	internal class HasNotpermissionException : HttpErrorBase
	{
		public HasNotpermissionException(string title, object? details = null) : base(title, details)
		{
		}

		protected override void Initialize()
		{
			this.StatusCode = 403;
		}
	}
}
