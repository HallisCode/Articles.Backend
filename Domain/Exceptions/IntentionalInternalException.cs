using System;

namespace Domain.Exceptions
{
	public class IntentionalInternalException : Exception
	{
		public int StatusCode { get; protected set; } = 500;

		public string Title { get; protected set; }

		public string? Details { get; protected set; }

		public IntentionalInternalException(string title, string? details = null)
			: base(title)
		{
			this.Title = title;

			this.Details = details;

			Initialize();
		}

		// Задаём в классе наследника характеристики для конкретной ошибки.
		protected virtual void Initialize()
		{

		}
	}

}
