using Application.ServicesBase.Security;
using Application.Utils;
using Database.Repositories;
using Domain.Entities.UserScope;
using Domain.Exceptions.Authentication;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Application.Services
{
	public sealed class SecurityService : SecurityServiceBase
	{
		private readonly UserSecurityRepository userSecurityRepository;

		private readonly UserSessionRepository userSessionRepository;


		public SecurityService(UserSecurityRepository userSecurityRepository, UserSessionRepository userSessionRepository)
		{
			this.userSecurityRepository = userSecurityRepository;

			this.userSessionRepository = userSessionRepository;

		}

		/// <summary>
		/// Меняем пароль установленному пользователю
		/// </summary>
		/// <returns></returns>
		/// <exception cref="CredentialDontMatchException"></exception>
		public override async Task ChangePasswordAsync(long userId, string oldPassword, string newPassword)
		{
			using (SHA256 sha256 = SHA256.Create())
			{
				oldPassword = SHA256Utils.EncryptToString(oldPassword, sha256);

				newPassword = SHA256Utils.EncryptToString(newPassword, sha256);
			}

			UserSecurity userSecurity = (await userSecurityRepository.TryGetByAsync(userId))!;

			if (userSecurity.Password == oldPassword)
			{
				await userSecurityRepository.UpdateAsync(userSecurity, null, newPassword);

				await userSessionRepository.DeleteAllAsync(userSecurity.UserId);
			}
			else
			{
				throw new CredentialDontMatchException("Old password isn't correct");
			}
		}


		public override async Task ChangeEmailAsync(long userId, string newEmail)
		{
			using (SHA256 sha256 = SHA256.Create())
			{
				newEmail = SHA256Utils.EncryptToString(newEmail.ToLower(), sha256);
			}

			UserSecurity userSecurity = (await userSecurityRepository.TryGetByAsync(userId))!;

			await userSecurityRepository.UpdateAsync(userSecurity, newEmail, null);

			await userSessionRepository.DeleteAllAsync(userSecurity.UserId);
		}
	}
}
