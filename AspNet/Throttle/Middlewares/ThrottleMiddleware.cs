using Application.IServices;
using AspNet.Throttle.Attrubites;
using AspNet.Throttle.Enum;
using AspNet.Throttle.Handlers;
using AspNet.Throttle.Options;
using Domain.Entities.UserScope;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace AspNet.Throttle.Middlewares
{
	public class ThrottleMiddleware
	{
		private readonly RequestDelegate next;

		private readonly IMemoryCache memoryCache;


		private Dictionary<Type, IThrottleHandler> handlers;

		private Dictionary<ThrottleGroup, IThrottleOptions> throttleGroupSettings;

		private string globalThrottlingKey = "global";


		public ThrottleMiddleware(RequestDelegate next, IMemoryCache memoryCache)
		{
			this.next = next;

			this.memoryCache = memoryCache;

			handlers = new Dictionary<Type, IThrottleHandler>();

			handlers[typeof(ThrottleWindowOptions)] = new ThrottleWindowHandler(memoryCache);

			throttleGroupSettings = new Dictionary<ThrottleGroup, IThrottleOptions>()
			{
				{ThrottleGroup.Anonymous, new ThrottleWindowOptions(1,60) },
				{ThrottleGroup.Identifier, new ThrottleWindowOptions(1,60)}
			};

		}

		public async Task InvokeAsync(HttpContext httpContext, IUserReciever<User> userReciever)
		{
			Endpoint? endpoint = httpContext.GetEndpoint();

			if (endpoint is null) await next.Invoke(httpContext);

			string identifier = GetIdentifier(httpContext, userReciever, out bool isAnonymous);

			string key = globalThrottlingKey;

			IThrottleOptions options = isAnonymous ? throttleGroupSettings[ThrottleGroup.Anonymous] : throttleGroupSettings[ThrottleGroup.Identifier];


			IThrottleAttribute<IThrottleOptions>? throttleAttribute = GetThrottleAttribute(endpoint);

			if (throttleAttribute is not null)
			{
				options = throttleAttribute.GetOptions();

				key = throttleAttribute.Key;
			}

			IThrottleHandler? handler = GetHandler(options);

			if (handler is null)
			{
				throw new Exception($"Для {options.GetType()} в {typeof(ThrottleMiddleware)} обработчик не задан.");
			}

			bool isThrottle = handler.Throttle(key, options);


			await next.Invoke(httpContext);
		}

		private string GetIdentifier(HttpContext httpContext, IUserReciever<User> userReciever, out bool isAnonymous)
		{
			isAnonymous = true;

			string key;


			User? user = userReciever.User;

			if (user is not null)
			{
				isAnonymous = false;

				key = user.Id.ToString();
			}
			else
			{
				key = httpContext.Request.Host.Host;
			}

			return key;
		}

		private IThrottleHandler? GetHandler(IThrottleOptions options)
		{
			return handlers.GetValueOrDefault(options.GetType());
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
