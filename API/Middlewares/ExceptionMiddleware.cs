using AspNet.Dto;
using Domain.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AspNet.Middlewares
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate next;

		public ExceptionMiddleware(RequestDelegate next)
		{
			this.next = next;

			Debug.WriteLine("middleware");
		}

		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await next.Invoke(httpContext);
			}
			catch (HttpErrorBase exception)
			{
				httpContext.Response.StatusCode = exception.StatusCode;

				ErrorDetails errorDetails = new ErrorDetails(exception.GetType().Name, exception.Title, exception.Details);

				await httpContext.Response.WriteAsJsonAsync(errorDetails);

				return;
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


