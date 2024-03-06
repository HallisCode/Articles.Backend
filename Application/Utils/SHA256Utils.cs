using System;
using System.Security.Cryptography;
using System.Text;

namespace Application.Utils
{
	public sealed class SHA256Utils
	{
		private SHA256Utils()
		{

		}

		/// <summary>
		/// Получает хэш данных с помощью алгоритма sha256, а затем переводит в строковое представление.
		/// </summary>
		/// <returns>Зашифрованные данные в представлении base64 (строка)</returns>
		public static string EncryptToString(string data, SHA256 sha256)
		{
			return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(data)));
		}
	}
}
