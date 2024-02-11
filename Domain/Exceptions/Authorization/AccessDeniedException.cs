
namespace Domain.Exceptions.Authorization
{
	public class AccessDeniedException : HttpErrorBase
	{
		public AccessDeniedException(string title, object? details = null) : base(title, details)
		{
		}

		protected override void Initialize()
		{
			this.StatusCode = 403;
		}
	}
}
