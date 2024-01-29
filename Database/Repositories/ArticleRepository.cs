using Domain.Entities.ArticleScope;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Database.Repositories
{
	public class ArticleRepository
	{
		private readonly ApplicationDbContext context;

		public ArticleRepository(ApplicationDbContext context)
		{
			this.context = context;
		}

		#region NullableMethods

		public async Task<Article?> TryGetByAsync(long id)
		{
			return await context.Articles.AsNoTracking()
				.Include(article => article.Tags).Include(article => article.Author)
				.SingleOrDefaultAsync(article => article.Id == id);
		}

		public async Task<Article?> TryGetByAsync(string title)
		{
			return await context.Articles.AsNoTracking()
				.Include(article => article.Tags).Include(article => article.Author)
				.FirstOrDefaultAsync(article => EF.Functions.Like(article.Title, $"%{title}%"));
		}

		public async Task<List<Article>?> TryGetByAsync(ICollection<Tag> tags)
		{
			return await context.Articles.AsNoTracking()
				.Include(article => article.Tags).Include(article => article.Author)
				.Where(article => article.Tags
				.Where(tag => tags.Contains(tag)).Count() == tags.Count())
				.ToListAsync();
		}

		#endregion

		#region NotNullableMethods

		public async Task<Article> CreateAsync(string title, string content, long userId, ICollection<Tag> tags)
		{
			Article article = new Article(userId, title, content, tags);

			article.CreatedAt = DateTime.UtcNow;

			article.Tags = tags;

			context.Articles.Add(article);

			await context.SaveChangesAsync();

			return article;
		}


		public async Task<Article> UpdateAsync(Article article, string? title = null, string? content = null, ICollection<Tag>? tags = null)
		{
			context.Add(article);

			if (title is not null) article.Title = title;

			if (content is not null) article.Content = content;

			if (tags is not null) article.Tags = tags;

			article.UpdatedAt = DateTime.UtcNow;

			await context.SaveChangesAsync();

			return article;
		}


		public async Task DeleteAsync(long id)
		{
			await context.Articles.Where(article => article.Id == id).ExecuteDeleteAsync();

			await context.SaveChangesAsync();
		}

		#endregion
	}
}
