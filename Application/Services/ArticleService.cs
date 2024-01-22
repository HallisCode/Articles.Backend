using Database.Repositories;
using Domain.Entities.ArticleScope;
using Domain.Exceptions;
using Domain.Exceptions.CRUD;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
	public class ArticleService
	{
		private readonly ArticleRepository articleRepository;

		private readonly TagRepository tagRepository;


		public ArticleService(ArticleRepository articleRepository, TagRepository tagRepository)
		{
			this.articleRepository = articleRepository;

			this.tagRepository = tagRepository;
		}

		/// <summary>
		/// Получаем статью на основе id
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NotFoundException"></exception>
		public async Task<Article> GetByAsync(long id)
		{
			Article? article = await articleRepository.TryGetByAsync(id);

			if (article is null) throw new NotFoundException("Article is not found");

			return article;
		}

		/// <summary>
		/// Получаем статью на основе title
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NotFoundException"></exception>
		public async Task<Article> GetByAsync(string title)
		{
			Article? article = await articleRepository.TryGetByAsync(title);

			if (article is null) throw new NotFoundException("Article is not found");

			return article;
		}

		/// <summary>
		/// Получаем статьи на основе совпадающих тегов
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NotFoundException"></exception>
		public async Task<List<Article>> GetByAsync(ICollection<long> tagsId)
		{
			List<Tag>? tags = await tagRepository.TryGetByAsync(tagsId);

			if (tags is null) throw new NotFoundException("No one tag isn't found");


			List<Article>? articles = await articleRepository.TryGetByAsync(tags);

			if (articles is null) throw new NotFoundException("Articles is not found");

			return articles;
		}

		public async Task<Article> CreateAsync(long userId, string title, string content, ICollection<long> tagsId)
		{
			Article? article = await articleRepository.TryGetByAsync(title);

			if (article is not null) throw new AlreadyExistException("Article with the same title is already exist");


			List<Tag>? tags = await tagRepository.TryGetByAsync(tagsId);

			if (tags is null) throw new NotFoundException("No one tag isn't found");


			tags = tags.Distinct().ToList();

			return await articleRepository.CreateAsync(title, content, userId, tags);
		}

		public async Task<Article> UpdateAsync(long id, string? title = null, string? content = null, ICollection<long>? tagsId = null)
		{
			Article? article = await articleRepository.TryGetByAsync(id);

			if (article is null) throw new NotFoundException("Article with this id isn't found");


			if (title is null && content is null && tagsId is null)
			{
				throw new ArgumentsMissingException("All arguments is null");
			}


			List<Tag>? tags = tagsId is null ? null : await tagRepository.TryGetByAsync(tagsId);

			return await articleRepository.UpdateAsync(article, title, content, tags);
		}

		public async Task DeleteAsync(long id)
		{
			Article? article = await articleRepository.TryGetByAsync(id);

			if (article is null) throw new NotFoundException("Articles isn't found");


			await articleRepository.DeleteAsync(id);
		}

	}
}
