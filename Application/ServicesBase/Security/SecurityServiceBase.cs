using Application.IServices.Security;
using System.Threading.Tasks;

namespace Application.ServicesBase.Security
{
	public abstract class SecurityServiceBase : ISecurityService
	{
		public abstract Task ChangeEmailAsync(long userId, string newEmail);

		public abstract Task ChangePasswordAsync(long userId, string oldPassword, string newPassword);
	}
}
