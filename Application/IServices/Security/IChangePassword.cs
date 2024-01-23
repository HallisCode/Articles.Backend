using System.Threading.Tasks;

namespace Application.IServices.Security
{
    /// <summary>
    /// <para>Интерфейс для смены почты и пароля.</para>
    /// </summary>
    public interface IChangePassword
    {
        /// <summary>
        /// Меняем пароль со старого на новый.
        /// </summary>
        /// <returns></returns>
        public Task ChangePasswordAsync(long userId, string oldPassword, string newPassword);
    }
}
