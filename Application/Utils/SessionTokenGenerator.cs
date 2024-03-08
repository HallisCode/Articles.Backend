using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils
{
	public class SessionTokenGenerator
	{
		public static string Generate()
		{
			byte[] data = new byte[256];

			using (RandomNumberGenerator random = RandomNumberGenerator.Create())
			{
				random.GetBytes(data);
			}

			return Convert.ToBase64String(data);
		}
	}
}
