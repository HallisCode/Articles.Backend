using Application.IServices.Security;
using System.Threading.Tasks;

namespace Application.ServicesBase.Security
{
	public abstract class SecurityServiceBase : IChangeEmail, IChangePassword
	{
		public abstract Task ChangeEmailAsync(long userId, string oldPassword, string newPassword);

		public abstract Task ChangePasswordAsync(long userId, string oldPassword, string newPassword);
	}
}
