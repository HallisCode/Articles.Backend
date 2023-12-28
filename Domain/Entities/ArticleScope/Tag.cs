using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.ArticleScope
{
	public class Tag
	{
		public int Id { get; set; }

		[MaxLength(16)]
		public string Title { get; set; }
	}
}
