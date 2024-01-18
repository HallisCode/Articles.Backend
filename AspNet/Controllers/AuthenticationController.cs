using Application.Services;
using AspNet.Dto.Response;
using AutoMapper;
using Domain.Entities.UserScope;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspNet.Controllers
{
    [ApiController]
	[Route("api/[controller]/[action]")]
	public class AuthenticationController : ControllerBase
	{
		private readonly AuthenticationService authenticationService;

		private IMapper mapper;


		public AuthenticationController(AuthenticationService authenticationService, IMapper mapper)
		{
			this.authenticationService = authenticationService;

			this.mapper = mapper;
		}

		[HttpPost]
		public async Task<ActionResult<UserDto>> Registry(string nickname, string email, string password, string? bio = null)
		{
			User user = await authenticationService.RegistryAsync(email, password, nickname, bio);

			return mapper.Map<User, UserDto>(user);

		}

		[HttpPost]
		public async Task<ActionResult<string>> LogIn(string email, string password)
		{
			return await authenticationService.LogInAsync(email, password);
		}
	}
}
