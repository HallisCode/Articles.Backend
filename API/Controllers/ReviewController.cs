using API.Options;
using Application.Services;
using AspNet.Dto.Request;
using AspNet.Dto.Response;
using AutoMapper;
using Domain.Entities.ArticleScope;
using Domain.Entities.UserScope;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace AspNet.Controllers
{
	[ApiController]
	[Route("api/[controller]/[action]")]
	public sealed class ReviewController : ControllerBase
	{
		private readonly ReviewService reviewService;

		private readonly IMapper mapper;

		private readonly IOptions<DataKeysOptions> dataKeys;

		public ReviewController(ReviewService reviewService, IOptions<DataKeysOptions> dataKeys, IMapper mapper)
		{
			this.reviewService = reviewService;

			this.mapper = mapper;

			this.dataKeys = dataKeys;
		}


		[HttpPost]
		public async Task<ActionResult<ReviewResponse>> CreateAsync([FromBody] ReviewRequest reviewRequest)
		{
			Review review = await reviewService.CreateAsync(
				user: (User)HttpContext.Items[dataKeys.Value.User]!,
				content: reviewRequest.Content,
				articleId: reviewRequest.ArticleId,
				type: reviewRequest.Type
				);

			return mapper.Map<Review, ReviewResponse>(review);
		}

		[HttpDelete]
		public async Task<ActionResult> DeleteAsync(long id)
		{
			await reviewService.DeleteAsync(
				user: (User)HttpContext.Items[dataKeys.Value.User]!,
				id: id
				);

			return Ok();
		}
	}
}
