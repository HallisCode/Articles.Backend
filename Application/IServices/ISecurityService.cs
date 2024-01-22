using System.Threading.Tasks;

namespace Application.IServices
{
	/// <summary>
	/// <para>Интерфейс для смены почты в 3 этапа :</para>
	/// <para>1. Создать токен для подтверждения смены почты для текущей почты.</para>
	/// <para>2. Создать токен для потверждения смены почты для новой почты, на основе предыдущего токена.</para>
	/// <para>3. Подтвердить смену почты на основе токена из пункта 2.</para>
	/// </summary>
	public interface ISecurityService
	{
		/// <summary>
		/// Меняем пароль со старого на новый.
		/// </summary>
		/// <returns></returns>
		public Task ChangePasswordAsync(long userId, string oldPassword, string newPassword);

		/// <summary>
		/// Создаём токен для подтверждения смены почты, на старой почте.
		/// </summary>
		/// <returns></returns>
		public Task<string> CreateTokenChangeEmailOnOldAsync(long userId, string oldEmail, string newEmail);

		/// <summary>
		/// Создаём токен для подтверждения смены почты, на новой почте.
		/// </summary>
		/// <returns></returns>
		public Task<string> CreateTokenChangeEmailOnNewAsync(string jwtTokenData);

		/// <summary>
		/// Подтверждаем токен на смену почты, полученный на новую почту.
		/// </summary>
		/// <returns></returns>
		public Task ConfirmEmailChangeAsync(string jwtTokenData);
	}
}
