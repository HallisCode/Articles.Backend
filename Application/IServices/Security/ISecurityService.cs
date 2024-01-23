using System.Threading.Tasks;

namespace Application.IServices.Security
{
    /// <summary>
    /// <para>Интерфейс для смены почты и пароля.</para>
    /// </summary>
    public interface ISecurityService
    {
        /// <summary>
        /// Меняем пароль со старого на новый.
        /// </summary>
        /// <returns></returns>
        public Task ChangePasswordAsync(long userId, string oldPassword, string newPassword);

        /// <summary>
        /// Меняем почту со старой на новую.
        /// </summary>
        /// <returns></returns>
        public Task ChangeEmaildAsync(long userId, string oldPassword, string newPassword);
    }
}
