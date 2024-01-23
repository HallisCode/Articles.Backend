using System.Threading.Tasks;

namespace Application.IServices.Security
{
    /// <summary>
    /// Сервис для смены почты с подтверждением на старой и новой почте.  
    /// <para>1. Отправить токен для подтверждения смены почты для текущей почты.</para>
    /// <para>2. Отправить токен для потверждения смены почты для новой почты, на основе предыдущего токена.</para>
    /// <para>3. Подтвердить смену почты на основе токена из пункта 2.</para>
    /// </summary>
    /// <typeparam name="TRequest">Представление токена, для подтверждения запросов</typeparam>
    public interface ISecurity2FAEmailService<TRequest>
    {
        /// <summary>
        /// Отправляем токен для подтверждения смены почты, на старой почте.
        /// </summary>
        /// <returns></returns>
        public Task SendTokenChangeEmailOnOldAsync(long userId, string oldEmail, string newEmail);

        /// <summary>
        /// Отправляем токен для подтверждения смены почты, на новой почте.
        /// </summary>
        /// <returns></returns>
        public Task SendTokenChangeEmailOnNewAsync(TRequest token);

        /// <summary>
        /// Подтверждаем токен на смену почты, полученный на новую почту.
        /// </summary>
        /// <returns></returns>
        public Task ConfirmEmailChangeAsync(TRequest token);
    }
}
