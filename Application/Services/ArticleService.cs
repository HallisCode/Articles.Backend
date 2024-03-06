using Database.Repositories;
using Domain.Entities.ArticleScope;
using Domain.Entities.UserScope;
using Domain.Exceptions;
using Domain.Exceptions.Authorization;
using Domain.Exceptions.CRUD;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
	public sealed class ArticleService
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

			if (article is null) throw new NotFoundException($"Статья с id - {id} не найдена.");

			return article;
		}

		/// <summary>
		/// Получаем статьи на основе title
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NotFoundException"></exception>
		public async Task<List<Article>?> GetByAsync(string title)
		{
			List<Article>? articles = await articleRepository.TryGetByAsync(title);

			if (articles is null) throw new NotFoundException("Ни одна статья с указанным заголовком не была найдена.");

			return articles;
		}

		/// <summary>
		/// Получаем статьи на основе совпадающих тегов
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NotFoundException"></exception>
		public async Task<List<Article>> GetByAsync(ICollection<long> tagsId)
		{
			List<Tag>? tags = await tagRepository.TryGetByAsync(tagsId);

			if (tags is null) throw new NotFoundException("Ни один указанный тег не был найден.");


			List<Article>? articles = await articleRepository.TryGetByAsync(tags);

			if (articles is null) throw new NotFoundException("Ни одна статья с указанными тегами не была найдена.");

			return articles;
		}

		/// <summary>
		/// Создаёт статью
		/// </summary>
		/// <returns></returns>
		/// <exception cref="AlreadyExistException"></exception>
		/// <exception cref="NotFoundException"></exception>
		public async Task<Article> CreateAsync(User user, string title, string content, ICollection<long> tagsId)
		{
			Article? article = (await articleRepository.TryGetByAsync(title))?.FirstOrDefault();

			if (article is not null) throw new AlreadyExistException("Статья с указанным заголовком уже существует.");


			List<Tag>? tags = await tagRepository.TryGetByAsync(tagsId);

			if (tags is null) throw new NotFoundException("Ни один указанный тег не был найден.");


			tags = tags.Distinct().ToList();

			return await articleRepository.CreateAsync(title, content, user.Id, tags);
		}

		/// <summary>
		/// Обновляет статью
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NotFoundException"></exception>
		/// <exception cref="ArgumentsMissingException"></exception>
		public async Task<Article> UpdateAsync(User user, long id, string title, string content, ICollection<long> tagsId)
		{
			Article? article = await articleRepository.TryGetByAsync(id);

			if (article is null) throw new NotFoundException($"Статья с id - {id} не найдена.");

			VerifyIsArticleOwner(user, article);


			List<Tag>? tags = await tagRepository.TryGetByAsync(tagsId);

			return await articleRepository.UpdateAsync(article, title, content, tags);
		}

		/// <summary>
		/// Удаляет статью
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NotFoundException"></exception>
		public async Task DeleteAsync(User user, long id)
		{
			Article? article = await articleRepository.TryGetByAsync(id);

			if (article is null) throw new NotFoundException($"Статья с id - {id} не найдена.");

			VerifyIsArticleOwner(user, article);


			await articleRepository.DeleteAsync(id);
		}

		/// <summary>
		/// Верификация того, яваляется ли пользователь автором article
		/// </summary>
		/// <exception cref="AccessDeniedException"></exception>
		private void VerifyIsArticleOwner(User user, Article article)
		{
			if (article.AuthorId != user.Id)
			{
				throw new AccessDeniedException(
					"Вы не имеете права удалить данную статью.",
					"Вы не являетесь автором данной стать."
				);
			}
		}

	}
}
