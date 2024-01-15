using Domain.Entities.ArticleScope;
using Domain.Entities.UserScope;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Database.Repositories
{
	public class ArticleRepository
	{
		private ApplicationDbContext context;

		public ArticleRepository(ApplicationDbContext context)
		{
			this.context = context;
		}

		// Get

		public async Task<Article?> GetByAsync(long id)
		{
			return await context.Articles.AsNoTracking().SingleOrDefaultAsync(article => article.Id == id);
		}

		public async Task<List<Article>?> GetByAsync(ICollection<Tag> tags)
		{
			return await context.Articles.AsNoTracking().Where(article => tags.All(tag => article.Tags.Contains(tag))).ToListAsync();
		}

		// Create

		public async Task<Article> CreateAsync(string title, string content, User user, ICollection<Tag> tags)
		{
			Article article = new Article(user, title, content, tags);

			article.Tags = tags;

			context.Articles.Add(article);

			await context.SaveChangesAsync();

			return article;
		}

		// Update

		public async Task<Article> UpdateAsync(Article article, string? title = null, string? content = null, ICollection<Tag>? tags = null)
		{
			context.Add(article);

			if (title is not null) article.Title = title;

			if (content is not null) article.Content = content;

			if (tags is not null) article.Tags = tags;

			article.UpdatedAt = DateTime.Now;

			await context.SaveChangesAsync();

			return article;
		}

		// Delete

		public async Task DeleteAsync(Article article)
		{
			context.Remove(article);

			await context.SaveChangesAsync();
		}
	}
}
