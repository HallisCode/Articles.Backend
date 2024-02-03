using System;

namespace Domain.Exceptions
{
	public abstract class HttpErrorBase : Exception
	{
		public int StatusCode { get; protected set; } = 500;

		public string Title { get; protected set; }

		public string? Details { get; protected set; }

		public HttpErrorBase(string title, string? details = null)
			: base()
		{
			this.Title = title;

			this.Details = details;

			Initialize();
		}

		// Задаём в классе наследника характеристики для конкретной ошибки.
		protected abstract void Initialize();
	}

}
