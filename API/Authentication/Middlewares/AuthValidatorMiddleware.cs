using API.Options;
using Application.IServices.Authentication;
using Domain.Entities.UserScope;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

namespace API.Authentication.Middlewares
{
	public class AuthValidatorMiddleware
	{
		private readonly RequestDelegate next;

		public AuthValidatorMiddleware(RequestDelegate next)
		{
			this.next = next;
		}

		public async Task InvokeAsync(HttpContext httpContext, IOptions<DataKeysOptions> dataKeys, IJWTAuthService<User, string> jWTAuthService)
		{
			// В случае присутствия в запросе jwt-token - валидируем его

			StringValues possibleJWTToken;

			bool isHasJWTToken = httpContext.Request.Headers.TryGetValue(dataKeys.Value.JWTToken, out possibleJWTToken);

			if (isHasJWTToken)
			{
				string jwtToken = possibleJWTToken[0]!;

				User user = await jWTAuthService.VerifyJWTTokenAsync(jwtToken);

				httpContext.Items[dataKeys.Value.User] = user;
			}

			await next.Invoke(httpContext);
		}

	}

	public static class AuthenticationValidatorMiddlewareExtension
	{
		public static void UseAuthValidatorMiddleware(this IApplicationBuilder app)
		{
			app.UseMiddleware<AuthValidatorMiddleware>();
		}
	}
}
