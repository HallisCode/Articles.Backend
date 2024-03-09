using Application.Configs;
using Application.IServices.Authentication;
using Application.Options;
using Application.ServicesBase.Authentication;
using Application.Utils;
using Database.Repositories;
using Domain.Entities.UserScope;
using Domain.Exceptions.Authentication;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Application.Services
{
	public sealed class AuthenticationSessionService : AuthentucationSessionServiceBase
	{
		private readonly UserSecurityRepository userSecurityRepository;

		private readonly UserRepository userRepository;

		private readonly UserSessionRepository sessionRepository;


		public AuthenticationSessionService(
			UserSecurityRepository userSecurityRepository,
			UserRepository userRepository,
			UserSessionRepository userSessionRepository
		)
		{
			this.userSecurityRepository = userSecurityRepository;

			this.userRepository = userRepository;

			this.sessionRepository = userSessionRepository;

		}

		/// <summary>
		/// Производит аутентификацию клиента
		/// </summary>
		/// <returns>jwtToken</returns>
		/// <exception cref="CredentialDontMatchException"></exception>
		public override async Task<string> LogInAsync(string email, string password, AuthOptions authOptions)
		{
			using (SHA256 sha256 = SHA256.Create())
			{
				email = SHA256Utils.EncryptToString(email.ToLower(), sha256);

				password = SHA256Utils.EncryptToString(password, sha256);
			}


			UserSecurity? userSecurity = await userSecurityRepository.TryGetByAsync(email);

			if (userSecurity is null) throw new CredentialDontMatchException("Почта или пароль указаны неверно");


			bool isPasswordMatched = userSecurity.Password == password;

			if (isPasswordMatched)
			{
				User user = (await userRepository.TryGetByAsync(userSecurity.UserId))!;


				List<UserSession> sessions = await sessionRepository.TryGetAllByAsync(user.Id);

				if (sessions.Count >= 16)
				{
					throw new SessionException("Превышено количество допустимых сессий");
				}


				UserSession? session = await sessionRepository.TryGetByAsync(user.Id, authOptions.AppName);

				if (session is null)
				{
					session = await sessionRepository.CreateAsync(user.Id,SessionTokenGenerator.Generate(), authOptions.AppName);
				}

				return session.Token;
				
			}

			throw new CredentialDontMatchException("Почта или пароль указаны неверно");
		}

		/// <summary>
		/// Производит удаление сессии на основе sessionId
		/// </summary>
		/// <returns></returns>
		public override async Task LogOutAsync(string token)
		{
			UserSession? session = await sessionRepository.TryGetByAsync(token);

			if (session is null)
			{
				throw new SessionException("Указанная сессия не существует");
			}

			await sessionRepository.DeleteAsync(session);
		}
	}

}
