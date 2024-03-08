using Application.Options;
using System.Threading.Tasks;

namespace Application.IServices.Authentication
{
	/// <summary>
	/// Сервис для аутентификации пользователя.
	/// </summary>
	/// <typeparam name="TToken">Тип данных идентифицирующих пользователя. 
	/// </typeparam>
	/// <typeparam name="TLoginResult"> В зависимости от реализации интерфейса, при выполнении LogIn, не всегда должны возвращаться данные.
	/// </typeparam>
	public interface IAuthenticationService<TToken, TLoginResult, IOptions>
	{
		/// <summary>
		/// Обрабатывает запрос пользователя на аутентификацию.
		/// </summary>
		/// <returns></returns>
		public Task<TLoginResult> LogInAsync(string email, string password, IOptions options);

		/// <summary>
		/// Производит выход из сесии.
		/// </summary>
		/// <returns></returns>
		public Task LogOutAsync(TToken token);
	}
}
