using System.ComponentModel.DataAnnotations;
using System;

namespace WebApi.Dto.ArticleScope
{
	public class ArticleDto
	{
		public long Id { get; set; }

		public string Title { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }

		public string Content { get; set; }
	}
}
