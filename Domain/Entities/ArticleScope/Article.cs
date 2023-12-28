using Domain.Entities.UserScope;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.ArticleScope
{
	public class Article
	{
		int Id { get; set; }

		User Author { get; set; }

		DateTime DateCreation { get; set; }

		[MaxLength(64)]
		string Title { get; set; }

		IEnumerable<Tag> Tags { get; set; }

		string Content { get; set; }
	}
}
