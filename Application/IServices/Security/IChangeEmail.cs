using System.Threading.Tasks;

namespace Application.IServices.Security
{
    /// <summary>
    /// <para>Интерфейс для смены почты и пароля.</para>
    /// </summary>
    public interface IChangeEmail
    {
        /// <summary>
        /// Меняем почту со старой на новую.
        /// </summary>
        /// <returns></returns>
        public Task ChangeEmailAsync(long userId, string oldPassword, string newPassword);
    }
}
