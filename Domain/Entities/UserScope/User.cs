using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.UserScope
{
	public class User
	{
		public int Id { get; set; }

		public Security Security { get; set; }

		[Range(4, 16)]
		public string Nickname { get; set; }

		public DateTime DateRegistration { get; set; }
	}
}
