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


		public static string Encrypt(string data, SHA256 sha256)
		{
			return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(data)));
		}
	}
}
