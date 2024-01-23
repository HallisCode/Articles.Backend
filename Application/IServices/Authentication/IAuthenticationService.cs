using System.Threading.Tasks;

namespace Application.IServices.Authentication
{
    /// <summary>
    /// Сервис для аутентификации пользователя.
    /// </summary>
    /// <typeparam name="TToken">Тип возвращаемых данных при запросе на аутентификацию. 
    /// Обычно это string, т.к в основном аутентификация реализауется за счёт jwt токенов и sessions.
    /// </typeparam>
    public interface IAuthenticationService<TToken>
    {
        /// <summary>
        /// Обрабатываем запрос пользователя на аутентификацию.
        /// </summary>
        /// <returns></returns>
        public Task<TToken> LogInAsync(string email, string password);

        /// <summary>
        /// Производим выход из сесии.
        /// </summary>
        /// <returns></returns>
        public Task LogOutAsync(string sessionId);
    }
}
