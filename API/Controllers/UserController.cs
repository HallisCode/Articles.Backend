using API.Authentication.Attrubites;
using Application.Services;
using AspNet.Dto.Response;
using AutoMapper;
using Domain.Entities.UserScope;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspNet.Controllers
{
	[AuthenticationNecessary]
	[ApiController]
	[Route("api/[controller]/[action]")]
	public sealed class UserController : ControllerBase
	{
		private readonly UserService userService;

		private readonly IMapper mapper;

		public UserController(UserService userService, IMapper mapper)
		{
			this.userService = userService;

			this.mapper = mapper;
		}

		[AllowAnonymous]
		[HttpGet]
		public async Task<UserResponse> GetById(long id)
		{
			User user = await userService.GetByAsync(id);

			return mapper.Map<User, UserResponse>(user);
		}

		[AllowAnonymous]
		[HttpGet]
		public async Task<UserResponse> GetByNickname(string nickname)
		{
			User user = await userService.GetByAsync(nickname);

			return mapper.Map<User, UserResponse>(user);
		}

	}
}
