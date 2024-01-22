using Application.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;

namespace AspNet.Middlewares
{
	public class SessionMiddleware
	{
		private readonly RequestDelegate next;

		public SessionMiddleware(RequestDelegate next)
		{
			this.next = next;
		}

		public async Task InvokeAsync(HttpContext httpContext, AuthenticationService authenticationService)
		{

			StringValues possibleSessionId;

			bool isHasSessionId = httpContext.Request.Headers.TryGetValue("sessionKey", out possibleSessionId);

			if (isHasSessionId)
			{
				string sessionKey = possibleSessionId[0]!;

				httpContext.Items["User"] = await authenticationService.CheckSessionId(sessionKey);
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
