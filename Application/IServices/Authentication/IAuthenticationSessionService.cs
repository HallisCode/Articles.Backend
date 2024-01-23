using System.Threading.Tasks;

namespace Application.IServices.Authentication
{
    /// <summary>
    /// Сервис для реализации проверки сессий.
    /// </summary>
    /// <typeparam name="TUser">Класс пользователя.</typeparam>
    public interface IAuthenticationSessionService<TUser>
        where TUser : class
    {
        /// <summary>
        /// Проверяет существование и действительность сессии.
        /// </summary>
        /// <returns>Пользователь данной сесии.</returns>
        public Task<TUser> CheckSessionId(string sessionId);
    }
}
