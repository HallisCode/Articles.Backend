using Application.Services;
using AspNet.Dto.Response;
using AutoMapper;
using Domain.Entities.ArticleScope;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{

    [ApiController]
	[Route("api/[controller]/[action]")]
	public class ArticleController : ControllerBase
	{
		private readonly ArticleService articleService;

		private readonly IMapper mapper;


		public ArticleController(ArticleService articleService, IMapper mapper)
		{
			this.articleService = articleService;

			this.mapper = mapper;
		}

		// Get

		[HttpGet()]
		public async Task<ActionResult<ArticleDto>> GetById(int id)
		{
			Article article = await articleService.GetByAsync(id);

			return mapper.Map<Article, ArticleDto>(article);
		}

		[HttpGet]
		public async Task<ActionResult<ICollection<ArticleDto>>> GetByTags([FromQuery] string[] tags)
		{
			List<Article> articles = await articleService.GetByAsync(tags);

			return mapper.Map<List<Article>, List<ArticleDto>>(articles);
		}

		// Create
	}
}
