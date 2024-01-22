﻿using Application.IServices;
using Application.Utils;
using Database.Repositories;
using Domain.Entities.UserScope;
using Domain.Exceptions.Authentication;
using Domain.Exceptions.CRUD;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Application.Services
{
	public class AuthenticationService : IAuthenticationService<User>
	{
		private readonly UserSecurityRepository userSecurityRepository;

		private readonly UserRepository userRepository;

		private readonly UserSessionRepository userSessionRepository;

		private readonly TimeSpan lifeSpanSession = new TimeSpan(21, 0, 0, 0);

		public AuthenticationService(
			UserSecurityRepository userSecurityRepository,
			UserRepository userRepository,
			UserSessionRepository userSessionRepository)
		{
			this.userSecurityRepository = userSecurityRepository;

			this.userRepository = userRepository;

			this.userSessionRepository = userSessionRepository;
		}

		/// <summary>
		/// Регистрирует пользователя на основе входных данных. 
		/// Создаются такие сущности как User, UserSecurity.
		/// </summary>
		public async Task<User> RegistryAsync(string email, string password, string nickname)
		{
			using (SHA256 sha256 = SHA256.Create())
			{
				email = SHA256Utils.Encrypt(email, sha256);

				password = SHA256Utils.Encrypt(password, sha256);
			}

			User? user;

			UserSecurity? userSecurity = await userSecurityRepository.TryGetByAsync(email);

			if (userSecurity is null)
			{
				user = await userRepository.TryGetByAsync(nickname);

				if (user is not null) throw new AlreadyExistException("This nickname is already claimed");


				user = await userRepository.CreateAsync(nickname);

				await userSecurityRepository.CreateAsync(email, password, user.Id);
			}
			else
			{
				throw new AlreadyExistException("This email is already registried");
			}

			return user;
		}

		/// <summary>
		/// Производит аутентификацию клиента
		/// </summary>
		/// <returns>SessionKey</returns>
		/// <exception cref="CredentialDontMatch"></exception>
		public async Task<string> LogInAsync(string email, string password)
		{
			using (SHA256 sha256 = SHA256.Create())
			{
				email = SHA256Utils.Encrypt(email, sha256);

				password = SHA256Utils.Encrypt(password, sha256);
			}


			UserSecurity? userSecurity = await userSecurityRepository.TryGetByAsync(email);

			if (userSecurity is null) throw new CredentialDontMatch("Email or password is wrong");


			bool isPasswordMatched = userSecurity.Password == password;

			if (isPasswordMatched)
			{
				UserSession? userSession = await userSessionRepository.TryGetByAsync(userSecurity.UserId);

				if (userSession is not null && DateTime.UtcNow < userSession.ExpiredAt) return userSession.SessionId;


				string sessionKey = SessionMaker.CreateSessionKey();

				await userSessionRepository.CreateAsync(sessionKey, userSecurity.UserId, DateTime.UtcNow + lifeSpanSession);

				return sessionKey;
			}

			throw new CredentialDontMatch("Email or password is wrong");
		}

		/// <summary>
		/// Производит удаление сессии на основе sessionId
		/// </summary>
		/// <returns></returns>
		/// <exception cref="SessionException"></exception>
		public async Task LogOutAsync(string sessionId)
		{
			UserSession? userSession = await userSessionRepository.TryGetByAsync(sessionId);

			if (userSession is null) throw new SessionException("Session isn't found");

			await userSessionRepository.DeleteAsync(sessionId);
		}

		/// <summary>
		/// Проверяет sessionId и на его основе возвращает пользователя
		/// </summary>
		/// <returns></returns>
		/// <exception cref="SessionException"></exception>
		public async Task<User> CheckSessionId(string sessionId)
		{
			UserSession? userSession = await userSessionRepository.TryGetByAsync(sessionId);

			if (userSession is null) throw new SessionException("Session isn't found");

			if (DateTime.UtcNow > userSession.ExpiredAt) throw new SessionException("Session is already expired");

			return (await userRepository.TryGetByAsync(userSession.UserId))!;
		}
	}
}
