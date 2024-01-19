using AspNet.Dto;
using Domain.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AspNet.Middlewares
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate next;

		public ExceptionMiddleware(RequestDelegate next)
		{
			this.next = next;
		}

		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await next.Invoke(httpContext);
			}
			catch (IntentionalInternalException exception)
			{
				httpContext.Response.StatusCode = exception.StatusCode;

				ErrorDetails errorDetails = new ErrorDetails(exception.GetType().Name, exception.Message);

				await httpContext.Response.WriteAsync(JsonSerializer.Serialize(errorDetails));
			}

			//catch (Exception exception)
			//{
			//	httpContext.Response.StatusCode = 500;

			//	ErrorDetails errorDetails = new ErrorDetails(
			//		nameof(IntentionalInternalException),
			//		"Ooops...something is wrong. Please, notify administrator of this website");

			//	await httpContext.Response.WriteAsync(JsonSerializer.Serialize(errorDetails));
			//}
		}
	}

	public static class ExceptionMiddlewareExtension
	{
		public static void UseExceptionMiddleware(this IApplicationBuilder app)
		{
			app.UseMiddleware<ExceptionMiddleware>();
		}
	}
}


