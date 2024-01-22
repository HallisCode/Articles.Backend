using Application.IServices;
using Application.Utils;
using Database.Repositories;
using Domain.Entities.UserScope;
using Domain.Exceptions.Authentication;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Application.Services
{
	public class SecurityService : ISecurityService
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
		public async Task ChangePasswordAsync(long userId, string oldPassword, string newPassword)
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

		public Task<string> CreateTokenChangeEmailOnOldAsync(long userId, string oldEmail, string newEmail)
		{
			throw new NotImplementedException();
		}

		public Task<string> CreateTokenChangeEmailOnNewAsync(string jwtTokenData)
		{
			throw new System.NotImplementedException();
		}

		public Task ConfirmEmailChangeAsync(string jwtTokenData)
		{
			throw new System.NotImplementedException();
		}
	}
}
