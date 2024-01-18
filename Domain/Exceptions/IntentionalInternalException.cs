using System;

namespace Domain.Exceptions
{
	public class IntentionalInternalException : Exception
	{
		public int StatusCode { get; protected set; } = 500;

		public string Message { get; protected set; }

		public IntentionalInternalException(string message)
			: base(message)
		{
			this.Message = message;

			Initialize();
		}

		// Задаём в классе наследника характеристики для конкретной ошибки.
		protected virtual void Initialize()
		{

		}
	}

}
