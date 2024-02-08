using Application.IServices;
using AspNet.Dto;
using AspNet.Throttle.Attrubites;
using AspNet.Throttle.Handlers;
using AspNet.Throttle.Options;
using Domain.Entities.UserScope;
using Domain.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AspNet.Throttle.Middlewares
{
	public class ThrottleMiddleware
	{
		private readonly RequestDelegate next;

		private readonly IMemoryCache memoryCache;


		private IReadOnlyCollection<IThrottleHandler> handlers;

		private IThrottleOptions authenticatedPolicy;

		private IThrottleOptions anonymousPolicy;

		private string globalThrottlingKey = "global";


		public ThrottleMiddleware(RequestDelegate next, IMemoryCache memoryCache)
		{
			this.next = next;

			this.memoryCache = memoryCache;

			handlers = (new List<IThrottleHandler>(handlers)
			{
				new ThrottleWindowHandler(memoryCache)
			}).AsReadOnly();


		}

		public async Task InvokeAsync(HttpContext httpContext, IUserReciever<User> userReciever)
		{
			Endpoint? endpoint = httpContext.GetEndpoint();

			if (endpoint is null)
			{
				await next.Invoke(httpContext);

				return;
			};


			string identifier = GetIdentifier(httpContext, userReciever, out bool isAnonymous);

			string policyKey = globalThrottlingKey;

			IThrottleOptions options = isAnonymous ? anonymousPolicy : authenticatedPolicy;


			IThrottleAttribute<IThrottleOptions>? throttleAttribute = GetThrottleAttribute(endpoint);

			if (throttleAttribute is not null)
			{
				options = throttleAttribute.GetOptions();

				policyKey = throttleAttribute.Key;
			}


			IThrottleHandler? handler = GetHandler(options);

			if (handler is null)
			{
				throw new Exception($"Для {options.GetType()} в {typeof(ThrottleMiddleware)} обработчик не задан.");
			}


			string key = $"{identifier}:{policyKey}";

			bool isThrottle = handler.Throttle(key, options);

			if (isThrottle)
			{
				SendError(httpContext, policyKey);

				return;
			}

			await next.Invoke(httpContext);
		}

		private void SendError(HttpContext httpContext, string policyKey)
		{
			TooManyRequestsException exception = new TooManyRequestsException(
				"Вы совершаете слишком много запросов.",
				$"Вам заблокированы все ресурсы, относящиеся к политике {policyKey}"
				);

			ErrorDetails errorDetails = new ErrorDetails(exception.GetType().Name, exception.Title, exception.Details);

			httpContext.Response.WriteAsJsonAsync(errorDetails);
		}

		private string GetIdentifier(HttpContext httpContext, IUserReciever<User> userReciever, out bool isAnonymous)
		{
			User? user = userReciever.User;

			string key = user?.Id.ToString() ?? httpContext.Request.Host.Host;

			isAnonymous = user is null;

			return key;
		}

		private IThrottleHandler? GetHandler(IThrottleOptions options)
		{
			return handlers.FirstOrDefault(handler => handler.OptionsType == options.GetType());
		}

		private IThrottleAttribute<IThrottleOptions>? GetThrottleAttribute(Endpoint endpoint)
		{
			ControllerActionDescriptor? endpointController = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();

			if (endpointController is null) throw new Exception("ControllerActionDescriptor не найден для текущего endpoint");


			TypeInfo controllerType = endpointController.ControllerTypeInfo;

			MethodInfo actionType = endpointController.MethodInfo;


			IThrottleAttribute<IThrottleOptions>? controllerThrottle = controllerType.GetCustomAttribute<ThrottleWindowAttibute>();

			IThrottleAttribute<IThrottleOptions>? actionThrottle = actionType.GetCustomAttribute<ThrottleWindowAttibute>();

			return actionThrottle ?? controllerThrottle;
		}
	}

	public static class ThrottleMiddlewareExtension
	{
		public static void UseThrottleMiddleware(this IApplicationBuilder app)
		{
			app.UseMiddleware<ThrottleMiddleware>();
		}
	}
}
