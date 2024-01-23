using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices.Security
{
	public interface ISecurityService
	{
		/// <summary>
		/// Меняем почту со старой на новую.
		/// </summary>
		/// <returns></returns>
		public Task ChangeEmailAsync(long userId, string oldEmail, string newEmail);

		/// <summary>
		/// Меняем пароль со старого на новый.
		/// </summary>
		/// <returns></returns>
		public Task ChangePasswordAsync(long userId, string oldPassword, string newPassword);
	}
}
