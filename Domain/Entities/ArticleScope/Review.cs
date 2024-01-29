using Domain.Entities.UserScope;
using Domain.Enum;
using System;
using System.Collections.Generic;

namespace Domain.Entities.ArticleScope
{
	public class Review
	{
		public long Id { get; set; }

		public string Content { get; set; }

		public ReviewType Type { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }


		#region Relationships

		public Article Article { get; set; }

		public long ArticleId { get; set; }

		public User Author { get; set; }

		public long UserId { get; set; }

		public ICollection<ReviewComment> ReviewComments { get; set; }

		#endregion


		#region Constructors

		private Review(string content, ReviewType type, DateTime createdAt, DateTime updatedAt)
		{
			this.Content = content;

			this.Type = type;

			this.CreatedAt = createdAt;

			this.UpdatedAt = updatedAt;
		}

		public Review(string content, ReviewType type, long authorId, long articleId)
		{
			this.Content = content;

			this.Type = type;

			this.UserId = authorId;

			this.ArticleId = articleId;

			this.CreatedAt = DateTime.UtcNow;
		}

		#endregion
	}
}
