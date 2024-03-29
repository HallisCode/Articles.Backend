﻿using System;

namespace AspNet.Dto.Response
{
	public class UserResponse
	{
		public long Id { get; set; }

		public string Nickname { get; set; }

		public DateTime RegistredAt { get; set; }

		public string? Bio { get; set; }
	}
}
