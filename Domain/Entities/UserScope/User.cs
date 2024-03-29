﻿using Domain.Entities.ArticleScope;
using Domain.Enum;
using System;
using System.Collections.Generic;

namespace Domain.Entities.UserScope
{
    public class User
	{
		public long Id { get; set; }

		public string Nickname { get; set; }

		public DateTime RegistredAt { get; set; }

		public UserRole Role { get; set; }

		public string Bio { get; set; }


		#region Relationships

		public UserSecurity UserSecurity { get; set; }

		public ICollection<UserSession> Sessions { get; set; }

		public ICollection<Article> Articles { get; set; }

		#endregion


		#region Constructors

		private User(string nickname, DateTime registredAt, UserRole role, string bio)
		{
			this.Nickname = nickname;

			this.RegistredAt = registredAt;

			this.Role = role;

			this.Bio = bio;
		}

		public User(string nickname, string? bio = null)
		{
			this.Nickname = nickname;

			this.Bio = bio;

			this.RegistredAt = DateTime.UtcNow;
		}

		#endregion
	}
}
