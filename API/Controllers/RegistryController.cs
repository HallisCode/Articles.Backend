using API.Authentication.Attrubites;
using Application.IServices.Registry;
using AspNet.Dto.Request;
using AspNet.Throttle.Attrubites;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspNet.Controllers
{
	[ThrottleResting("registry", 4, 256)]
	[AuthenticationNecessary]
	[ApiController]
	[Route("api/[controller]/[action]")]
	public sealed class RegistryController : ControllerBase
	{
		private readonly IRegistryService registryService;

		public RegistryController(IRegistryService registryService)
		{
			this.registryService = registryService;

		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<ActionResult> Registry([FromBody] RegistryRequest registryRequest)
		{
			await registryService.RegistryAsync(
				email: registryRequest.Email,
				password: registryRequest.Password,
				nickname: registryRequest.Nickname
				);

			return Ok();
		}
	}
}
