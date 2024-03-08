using API.Options;
using Application.IServices.Authentication;
using Domain.Entities.UserScope;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Reflection;
using System;
using System.Threading.Tasks;
using Domain.Exceptions.Authorization;
using API.Authentication.Attrubites;

namespace API.Authentication.Middlewares
{
	public enum AccessMode
	{
		Strong,
		Soft
	}

	public class AuthenticationMiddleware
	{
		private readonly RequestDelegate next;

		private readonly AccessMode mode;


		public AuthenticationMiddleware(RequestDelegate next, AccessMode mode)
		{
			this.next = next;

			this.mode = mode;
		}

		public async Task InvokeAsync(
			HttpContext httpContext, 
			IOptions<HeaderKeys> headerKeys, 
			IOptions<HttpContextKeys> httpContextKeys,
			ISessionService<User, string> sessionService
		)
		{
			Endpoint? endpoint = httpContext.GetEndpoint();

			User? user = null;


			// В случае присутствия в запросе jwt-token - валидируем его

			StringValues possibleSessionToken;

			bool isHasSessionToken = httpContext.Request.Headers.TryGetValue(headerKeys.Value.Session, out possibleSessionToken);

			if (isHasSessionToken)
			{
				string token = possibleSessionToken[0]!;

				user = await sessionService.VerifySession(token);

				httpContext.Items[httpContextKeys.Value.User] = user;
			}


			if (endpoint is null)
			{
				await next.Invoke(httpContext);

				return;
			};


			// Проверяем при необходимости, аутентифицирован пользователь или нет.

			bool isNecessaryAuth = CheckNecessaryAuthentication(endpoint);

			if (isNecessaryAuth && user is null)
			{
				throw new AccessDeniedException(
					"Для доступа к ресурсу нужно быть аутентифицированным.",
					$"Передайте в заголовок запроса параметр {headerKeys.Value.Session}"
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


			switch (mode)
			{
				case (AccessMode.Soft):

					return isAllowAnonymous ? false : (isActionRequire || isControllerRequire);

				case (AccessMode.Strong):

					return !isAllowAnonymous;
			}

			return false;
		}

	}

	public static class AuthenticationMiddlewareExtension
	{
		public static void UseAuthenticationMiddleware(this IApplicationBuilder app, AccessMode mode = AccessMode.Strong)
		{
			app.UseMiddleware<AuthenticationMiddleware>(mode);
		}
	}
}
