using System.ComponentModel.DataAnnotations;
using System;

namespace WebApi.Dto.UserScope
{
	public class UserDto
	{
		public long Id { get; set; }

		public string Nickname { get; set; }

		public DateTime RegistredAt { get; set; }

		public string? Bio { get; set; }
	}
}
