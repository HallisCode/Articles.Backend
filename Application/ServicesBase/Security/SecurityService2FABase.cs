using Application.IServices.Security;
using System.Threading.Tasks;

namespace Application.ServicesBase.Security
{
	/// <summary>
	/// Представляет семейство сервисов изменения данных безопасности, где почта меняется через подтверждение.
	/// </summary>
	/// <typeparam name="TRequest"></typeparam>
	public abstract class SecurityService2FABase<TRequest> : ISecurityService,
		IConfirmChangeEmail<TRequest>
	{
		public abstract Task ChangePasswordAsync(long userId, string oldPassword, string newPassword);

		public abstract Task ChangeEmailAsync(long userId, string newEmail);


		protected abstract Task SendTokenChangeEmailOnOldEmailAsync(long userId, string oldEmail, string newEmail);

		protected abstract Task SendTokenChangeEmailOnNewEmailAsync(long userId, string newEmail);


		public abstract Task ConfirmChangeOnOldEmailAsync(TRequest token);

		public abstract Task ConfirmChangeOnNewEmailAsync(TRequest token);
	}
}
