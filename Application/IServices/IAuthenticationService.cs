using System;
using System.Threading.Tasks;

namespace Application.IServices
{
	/// <summary>
	/// Сервис для аутентификации пользователя на основе сессий.
	/// </summary>
	/// <typeparam name="TUser">Класс пользователя.</typeparam>
	public interface IAuthenticationService<TUser>
	{
		/// <summary>
		/// Регистрируем пользователя.
		/// </summary>
		/// <returns>Возвращаем созданный аккаунт.</returns>
		public Task<TUser> RegistryAsync(string email, string password, string nickname);

		/// <summary>
		/// Производим аутентификацию пользователя.
		/// </summary>
		/// <returns>Идентификатор сессии (sessionId)</returns>
		public Task<string> LogInAsync(string email, string password);

		/// <summary>
		/// Производим выход из сесии путём её удаления.
		/// </summary>
		/// <returns></returns>
		public Task LogOutAsync(string sessionId);

		/// <summary>
		/// Проверяет существование и действительность сессии.
		/// </summary>
		/// <returns>Пользователь данной сесии.</returns>
		public Task<TUser> CheckSessionId(string sessionId);

	}
}
