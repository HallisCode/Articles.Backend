using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices.Security
{
	public interface IConfirmChangeEmail<TRequest> : IConfirmChangeByOldEmail<TRequest>, IConfirmChangeByNewEmail<TRequest>
	{
	}
}
