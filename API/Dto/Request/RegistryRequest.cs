﻿namespace AspNet.Dto.Request
{
	public class RegistryRequest
	{
		public string Nickname { get; set; }

		public string Email { get; set; }

		public string Password { get; set; }

		public string? Bio { get; set; }
	}
}
