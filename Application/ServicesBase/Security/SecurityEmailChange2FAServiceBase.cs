using System.Threading.Tasks;
using Application.IServices.Security;

namespace Application.ServicesBase.Security
{

    /// <summary>
    /// <para>НАРАБОТКА, НИГДЕ НЕ ИСПОЛЬЗОВАТЬ</para>
    /// Сервис для смены почты с подтверждением на старой и новой почте.  
    /// <para>1. Отправить токен для подтверждения смены почты для текущей почты.</para>
    /// <para>2. Отправить токен для потверждения смены почты для новой почты, на основе предыдущего токена.</para>
    /// <para>3. Подтвердить смену почты на основе токена из пункта 2.</para>
    /// </summary>
    /// <typeparam name="TRequest">Представление токена, для подтверждения запросов</typeparam>
    public abstract class SecurityEmailChange2FAServiceBase<TRequest> : IChangeEmail, IConfirmChange<TRequest>
    {
        public abstract Task ChangeEmailAsync(long userId, string oldPassword, string newPassword);

        /// <summary>
        /// Отправляем токен для подтверждения смены почты, на старой почте.
        /// </summary>
        /// <returns></returns>
        protected abstract Task SendTokenChangeEmailOnOldAsync(long userId, string oldEmail, string newEmail);

        /// <summary>
        /// Отправляем токен для подтверждения смены почты, на новой почте.
        /// </summary>
        /// <returns></returns>
        public abstract Task SendTokenChangeEmailOnNewAsync(TRequest token);

        /// <summary>
        /// Подтверждаем токен на смену почты, полученный на новую почту.
        /// </summary>
        /// <returns></returns>
        public abstract Task ConfirmChangeAsync(TRequest token);
    }
}
