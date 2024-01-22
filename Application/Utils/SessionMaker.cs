using System;
using System.Security.Cryptography;

namespace Application.Utils
{
	public sealed class SessionMaker
	{
		private SessionMaker()
		{

		}

		public static string CreateSessionKey()
		{
			using (var random = RandomNumberGenerator.Create())
			{
				byte[] id = new byte[64]; // 512 бит = 64 байта

				random.GetBytes(id);

				// Преобразование в Base64 строку
				string base64String = Convert.ToBase64String(id);

				return base64String;
			}
		}
	}
}
