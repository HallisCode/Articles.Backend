using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices.Security
{
	/// <summary>
	/// Подтверждает изменения, на основе токена, полученного со старой почты.
	/// </summary>
	/// <typeparam name="TRequest"> Представление токена, для подтверждения выполнения запросов.</typeparam>
	public interface IConfirmChangeOnOldEmail<TRequest>
	{
		public Task ConfirmChangeOnOldEmailAsync(TRequest token);
	}
}
