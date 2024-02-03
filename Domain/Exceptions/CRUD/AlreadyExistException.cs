namespace Domain.Exceptions.CRUD
{
	public class AlreadyExistException : HttpErrorBase
	{
		public AlreadyExistException(string title, string? details = null) : base(title, details)
		{
		}

		protected override void Initialize()
		{
			StatusCode = 409;
		}


	}
}
