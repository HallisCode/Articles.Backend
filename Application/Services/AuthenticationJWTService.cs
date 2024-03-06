using Application.IServices.Authentication;
using Application.Options;
using Application.ServicesBase.Authentication;
using Application.Utils;
using Database.Repositories;
using Domain.Entities.UserScope;
using Domain.Enum;
using Domain.Exceptions.Authentication;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using JWT.Serializers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Application.Services
{
	public sealed class AuthenticationJWTService : AutheticationServiceBase<string>, IJWTAuthService<User, string>
	{
		private readonly UserSecurityRepository userSecurityRepository;

		private readonly UserRepository userRepository;


		private readonly IOptions<JWTOptions> options;

		public AuthenticationJWTService(
			UserSecurityRepository userSecurityRepository,
			UserRepository userRepository,
			IOptions<JWTOptions> options)
		{
			this.userSecurityRepository = userSecurityRepository;

			this.userRepository = userRepository;

			this.options = options;
		}

		/// <summary>
		/// Производит аутентификацию клиента
		/// </summary>
		/// <returns>jwtToken</returns>
		/// <exception cref="CredentialDontMatchException"></exception>
		public override async Task<string> LogInAsync(string email, string password)
		{
			using (SHA256 sha256 = SHA256.Create())
			{
				email = SHA256Utils.EncryptToString(email.ToLower(), sha256);

				password = SHA256Utils.EncryptToString(password, sha256);
			}


			UserSecurity? userSecurity = await userSecurityRepository.TryGetByAsync(email);

			if (userSecurity is null) throw new CredentialDontMatchException("Email or password is wrong");


			bool isPasswordMatched = userSecurity.Password == password;

			if (isPasswordMatched)
			{
				User user = (await userRepository.TryGetByAsync(userSecurity.UserId))!;

				JWTPayload payload = new JWTPayload(
					exp: DateTimeOffset.Now.AddDays(options.Value.LifeSpanDays).ToUnixTimeSeconds(),
					userId: user.Id,
					role: user.Role
				);


				IJwtEncoder encoder = JWTEncoder.Create();

				string token = encoder.Encode(payload, options.Value.SignatureKey);

				return token;
			}

			throw new CredentialDontMatchException("Email or password is wrong");
		}

		/// <summary>
		/// Производит удаление сессии на основе sessionId
		/// </summary>
		/// <returns></returns>
		public override async Task LogOutAsync(string jwtToken)
		{

		}

		public async Task<User> VerifyJWTTokenAsync(string jwtToken)
		{
			try
			{
				IJwtDecoder decoder = JWTDecoder.Create();

				JWTPayload payload = decoder.DecodeToObject<JWTPayload>(jwtToken, options.Value.SignatureKey);


				User user = (await userRepository.TryGetByAsync(payload.UserId))!;

				return user;
			}
			catch (TokenNotYetValidException)
			{
				throw new JWTException("Невалидный токен.");
			}
			catch (TokenExpiredException)
			{
				throw new JWTException("Срок действия токена истёк.");
			}
			catch (SignatureVerificationException)
			{
				throw new JWTException("Невалидная подпись токена.");
			}
		}

		private class JWTPayload
		{
			public long UserId { get; set; }

			public UserRole Role { get; set; }

			public long Exp { get; set; }

			public JWTPayload(long userId, UserRole role, long exp)
			{
				this.UserId = userId;

				this.Role = role;

				this.Exp = exp;
			}
		}
	}

}
