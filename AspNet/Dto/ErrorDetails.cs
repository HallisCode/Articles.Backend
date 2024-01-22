namespace AspNet.Dto
{
	public class ErrorDetails
	{
		public string TypeError { get; private set; }

		public string Title { get; private set; }

		public string? Details { get; private set; }

		public ErrorDetails(string typeError, string title, string? details = null)
		{
			this.TypeError = typeError;

			this.Title = title;

			this.Details = details;
		}

	}
}
