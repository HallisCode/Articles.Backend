using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.Authorization
{
	internal class HasNotpermissionException : HttpErrorBase
	{
		public HasNotpermissionException(string title, string? details = null) : base(title, details)
		{
		}

		protected override void Initialize()
		{
			this.StatusCode = 403;
		}
	}
}
