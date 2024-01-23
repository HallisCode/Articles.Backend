using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
	/// <summary>
	/// Сервис для регистрации нового пользователя.
	/// </summary>
	public interface IRegistryService
	{
		public Task RegistryAsync(string email, string password, string nickname);
	}
}
