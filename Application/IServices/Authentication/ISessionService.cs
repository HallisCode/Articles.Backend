using System.Threading.Tasks;

namespace Application.IServices.Authentication
{
	/// <summary>
	/// Сервис для проверки сессий.
	/// </summary>
	/// <typeparam name="TUser">Класс пользователя.</typeparam>
	/// <typeparam name="TSession">Класс сессии/идентификатора</typeparam>
	public interface ISessionService<TUser, TSession>
        where TUser : class
    {
        /// <summary>
        /// Проверяет существование и действительность сессии.
        /// </summary>
        /// <returns>Пользователь данной сесии.</returns>
        public Task<TUser> CheckSession(TSession session);
    }
}
