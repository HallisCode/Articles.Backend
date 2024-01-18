namespace AspNet.Dto
{
	public class ErrorDetails
	{
		public string TypeError { get; private set; }

		public string Message { get; private set; }

		public ErrorDetails(string typeError, string message)
		{
			this.TypeError = typeError;

			this.Message = message;
		}

	}
}
