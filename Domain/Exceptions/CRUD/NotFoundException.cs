namespace Domain.Exceptions.CRUD
{
	public class NotFoundException : HttpErrorBase
	{
		public NotFoundException(string title, string? details = null) : base(title, details)
		{
		}

		protected override void Initialize()
		{
			StatusCode = 404;
		}
	}
}
