using Application.IServices.Authentication;
using AspNet.Attrubites;
using AspNet.Dto.Request;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspNet.Controllers
{
    [Authorize]
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class AuthenticationController : ControllerBase
	{
		private readonly IAuthenticationService<string, string> authenticationService;


		public AuthenticationController(IAuthenticationService<string, string> authenticationService)
		{
			this.authenticationService = authenticationService;

		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<ActionResult<string>> LogIn(LogInRequest logInModel)
		{
			return await authenticationService.LogInAsync(
				logInModel.Email,
				logInModel.Password
				);
		}

		[HttpDelete]
		public async Task<ActionResult> LogOut([FromBody] string sessiondId)
		{
			await authenticationService.LogOutAsync(sessiondId);

			return Ok();
		}
	}
}
