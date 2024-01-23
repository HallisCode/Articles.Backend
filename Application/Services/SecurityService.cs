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
    public class SecurityService : SecurityServiceBase
	{
		private readonly UserSecurityRepository userSecurityRepository;

		private readonly UserSessionRepository userSessionRepository;

		private readonly TimeSpan lifeSpanToken = new TimeSpan(0, 5, 0);


		public SecurityService(UserSecurityRepository userSecurityRepository, UserSessionRepository userSessionRepository)
		{
			this.userSecurityRepository = userSecurityRepository;

			this.userSessionRepository = userSessionRepository;
		}

		/// <summary>
		/// Меняем пароль установленному пользователю
		/// </summary>
		/// <returns></returns>
		/// <exception cref="CredentialDontMatch"></exception>
		public override async Task ChangePasswordAsync(long userId, string oldPassword, string newPassword)
		{
			using (SHA256 sha256 = SHA256.Create())
			{
				oldPassword = SHA256Utils.Encrypt(oldPassword, sha256);

				newPassword = SHA256Utils.Encrypt(newPassword, sha256);
			}

			UserSecurity userSecurity = (await userSecurityRepository.TryGetByAsync(userId))!;

			if (userSecurity.Password == oldPassword)
			{
				await userSecurityRepository.UpdateAsync(userSecurity, null, newPassword);

				// TODO : реализовать с реализацией мультисессий, удаление всех сессий

				await userSessionRepository.DeleteAsync(userId);
			}
			else
			{
				throw new CredentialDontMatch("Old password isn't correct");
			}
		}

		public override Task ChangeEmailAsync(long userId, string oldPassword, string newPassword)
		{
			throw new NotImplementedException();
		}
	}
}
