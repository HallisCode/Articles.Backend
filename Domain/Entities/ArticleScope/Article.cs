using Domain.Entities.UserScope;
using System;
using System.Collections.Generic;


namespace Domain.Entities.ArticleScope
{
	public class Article
	{
		public long Id { get; set; }

		public string Title { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }

		public string Content { get; set; }


		#region Relationships

		public User Author { get; set; }

		public long AuthorId { get; set; }

		public ICollection<Tag> Tags { get; set; }

		public ICollection<Review> Reviews { get; set; }

		#endregion


		#region Constructors

		private Article(string title, DateTime createdAt, DateTime updatedAt, string content)
		{
			this.Title = title;

			this.CreatedAt = createdAt;

			this.UpdatedAt = updatedAt;

			this.Content = content;
		}

		public Article(long authorId, string title, string content, ICollection<Tag> tags)
		{
			this.AuthorId = authorId;

			this.Title = title;

			this.Content = content;

			this.Tags = new List<Tag>();

			this.Tags = tags;

			this.CreatedAt = DateTime.UtcNow;
		}

		#endregion

	}
}
