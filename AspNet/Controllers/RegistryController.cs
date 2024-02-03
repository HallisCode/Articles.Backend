using Application.IServices.Registry;
using AspNet.Authorization.Attrubites;
using AspNet.Dto.Request;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspNet.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class RegistryController : ControllerBase
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
