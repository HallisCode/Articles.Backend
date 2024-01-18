using System.Collections.Generic;

namespace Domain.Entities.ArticleScope
{
	public class Tag
	{
		public int Id { get; set; }

		public string Title { get; set; }

		#region Relationships

		public ICollection<Article> Articles { get; set; }

		#endregion

		public Tag(string title)
		{
			this.Title = title;
		}
	}
}
