using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
	public class ValidationException : HttpErrorBase
	{
		public ValidationException(string title, object? details = null) : base(title, details)
		{
		}

		protected override void Initialize()
		{
			this.StatusCode = 400;
		}
	}
}
