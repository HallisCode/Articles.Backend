using System;

namespace Domain.Entities.UserScope
{
	public class UserSession
	{
		public long Id { get; set; }

		public string SessionKey { get; set; }

		public DateTime ExpiredAt { get; set; }

		#region Relationships

		public User User { get; set; }

		public long UserId { get; set; }

		#endregion

		#region Constructors

		private UserSession(string sessionKey, DateTime expiredAt)
		{
			this.SessionKey = sessionKey;

			this.ExpiredAt = expiredAt;

		}

		public UserSession(string sessionKey, long userId, DateTime expiredAt)
		{
			this.SessionKey = sessionKey;

			this.ExpiredAt = expiredAt;

			this.UserId = userId;
		}

		#endregion
	}
}
