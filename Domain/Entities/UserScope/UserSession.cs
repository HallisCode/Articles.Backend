using System;

namespace Domain.Entities.UserScope
{
	public class UserSession
	{
		public long Id { get; set; }

		public string SessionId { get; set; }

		public DateTime ExpiredAt { get; set; }

		#region Relationships

		public User User { get; set; }

		public long UserId { get; set; }

		#endregion

		#region Constructors

		private UserSession(string sessionId, DateTime expiredAt)
		{
			this.SessionId = sessionId;

			this.ExpiredAt = expiredAt;

		}

		public UserSession(string sessionId, long userId, DateTime expiredAt)
		{
			this.SessionId = sessionId;

			this.ExpiredAt = expiredAt;

			this.UserId = userId;
		}

		#endregion
	}
}
