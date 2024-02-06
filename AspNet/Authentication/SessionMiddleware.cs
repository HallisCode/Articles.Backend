using Application.IServices;
using Application.IServices.Authentication;
using Domain.Entities.UserScope;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

namespace AspNet.Authentication
{
	public class SessionMiddleware
	{
		private readonly RequestDelegate next;

		public SessionMiddleware(RequestDelegate next)
		{
			this.next = next;
		}

		public async Task InvokeAsync(HttpContext httpContext, ISessionService<User, string> authenticationService)
		{
			StringValues possibleSessionId;

			bool isHasSessionId = httpContext.Request.Headers.TryGetValue("sessionId", out possibleSessionId);

			if (isHasSessionId)
			{
				string sessionId = possibleSessionId[0]!;

				User user = await authenticationService.VerifySession(sessionId);

				httpContext.Items["User"] = user;
			}

			await next.Invoke(httpContext);
		}
	}

	public static class SessionMiddlewareExtension
	{
		public static void UseSessionMiddlewar(this IApplicationBuilder app)
		{
			app.UseMiddleware<SessionMiddleware>();
		}
	}
}
