using System.Threading.Tasks;

namespace Application.IServices.Registry
{
	/// <summary>
	/// Сервис для регистрации нового пользователя.
	/// </summary>
	public interface IRegistryService
	{
		public Task RegistryAsync(string email, string password, string nickname);
	}
}
