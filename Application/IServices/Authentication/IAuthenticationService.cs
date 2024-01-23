using System.Threading.Tasks;

namespace Application.IServices.Authentication
{
	/// <summary>
	/// Сервис для аутентификации пользователя.
	/// </summary>
	/// <typeparam name="TToken">Тип данных идентифицирующих пользователя. 
	/// </typeparam>
	/// <typeparam name="TLoginResult"> В зависимости от реализации интерфейса, при выполнении LogIn, не всегда должны возвращаться данные.
    /// Должен быть таким же как TToken либо object (void).</typeparam>
	public interface IAuthenticationService<TToken, TLoginResult>
    {
        /// <summary>
        /// Обрабатываем запрос пользователя на аутентификацию.
        /// </summary>
        /// <returns></returns>
        public Task<TLoginResult> LogInAsync(string email, string password);

        /// <summary>
        /// Производим выход из сесии.
        /// </summary>
        /// <returns></returns>
        public Task LogOutAsync(TToken token);
    }
}
