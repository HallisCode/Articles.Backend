using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.UserScope
{
    public class UserSession
    {
        public long Id { get; set; }

        public string Token { get; set; }

        public string AppName { get; set; }

        public DateTime CreatedAt { get; set; }

		public DateTime LastActivity { get; set; }


        #region Relationships

        public User User { get; set; }

        public long UserId { get; set; }

		#endregion

		#region Constructors

		private UserSession(string token, string appName, DateTime createdAt, DateTime lastActivity)
		{
			this.Token = token;

			this.AppName = appName;

			this.CreatedAt = createdAt;

			this.LastActivity = lastActivity;
		}

		public UserSession(long userId, string token, string appName)
		{
			this.UserId = userId;

			this.Token = token;

			this.AppName = appName;

			this.CreatedAt = DateTime.UtcNow;

			this.LastActivity = DateTime.UtcNow;
		}


		#endregion
	}
}
