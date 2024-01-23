using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices.Security
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TRequest"> Представление токена, для подтверждения выполнения запросов.</typeparam>
	public interface IConfirmChange<TRequest>
	{
		public Task ConfirmChangeAsync(TRequest token);
	}
}
