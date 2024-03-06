using API.Authentication.Attrubites;
using API.Options;
using Domain.Entities.UserScope;
using Domain.Exceptions.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace API.Authentication.Middlewares
{
	public class VerifyNecessaryAuthMiddleware
	{
		private readonly RequestDelegate next;

		public VerifyNecessaryAuthMiddleware(RequestDelegate next)
		{
			this.next = next;
		}

		public async Task InvokeAsync(HttpContext httpContext, IOptions<DataKeysOptions> dataKeys)
		{
			// Проверяем условия, нужно ли быть аутентифицированным пользователем или нет

			Endpoint? endpoint = httpContext.GetEndpoint();

			if (endpoint is null)
			{
				await next.Invoke(httpContext);

				return;
			};


			bool isNecessaryAuth = CheckNecessaryAuthentication(endpoint);

			if (!isNecessaryAuth)
			{
				await next.Invoke(httpContext);

				return;
			}


			User? user = (User?)httpContext.Items[dataKeys.Value.User];

			if (isNecessaryAuth && user is null)
			{
				throw new AccessDeniedException(
					"Для доступа к ресурсу нужно быть аутентифицированным.",
					$"Передайте в заголовок запроса параметр {dataKeys.Value.JWTToken}"
				);
			}

			await next.Invoke(httpContext);
		}

		private bool CheckNecessaryAuthentication(Endpoint endpoint)
		{
			ControllerActionDescriptor? endpointController = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();

			if (endpointController is null) throw new Exception($"{typeof(ControllerActionDescriptor)} не найден для текущего endpoint");


			TypeInfo controllerType = endpointController.ControllerTypeInfo;

			MethodInfo actionType = endpointController.MethodInfo;


			bool isActionRequire = actionType.GetCustomAttribute(typeof(AuthenticationNecessaryAttribute), false)
				is null ? false : true;

			bool isControllerRequire = controllerType.GetCustomAttribute(typeof(AuthenticationNecessaryAttribute), false)
				is null ? false : true;

			bool isAllowAnonymous = actionType.GetCustomAttribute(typeof(AllowAnonymousAttribute), false)
				is null ? false : true;


			return isAllowAnonymous ? false : (isActionRequire || isControllerRequire);
		}
	}

	public static class VerifyNecessaryAuthMiddlewareExtension
	{
		public static void UseVerifyNecessaryAuthMiddleware(this IApplicationBuilder app)
		{
			app.UseMiddleware<VerifyNecessaryAuthMiddleware>();
		}
	}
}
