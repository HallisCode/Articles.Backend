using Domain.Entities.ArticleScope;
using System.Collections.Generic;

namespace AspNet.Dto.Request
{
	public class ArticleRequest
	{
		public string Title { get; set; }

		public string Content { get; set; }

		public ICollection<long> Tags { get; set; }
	}
}
