using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
	public class TooManyRequestsException : HttpErrorBase
	{
		public TooManyRequestsException(string title, string? details = null) : base(title, details)
		{
		}

		protected override void Initialize()
		{
			StatusCode = 429;
		}
	}
}
