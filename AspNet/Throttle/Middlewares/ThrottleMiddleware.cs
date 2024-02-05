using Application.IServices;
using AspNet.Dto;
using AspNet.Throttle.Attrubites;
using AspNet.Throttle.Enum;
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
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace AspNet.Throttle.Middlewares
{
	public class ThrottleMiddleware
	{
		private readonly RequestDelegate next;

		private readonly IMemoryCache memoryCache;

		private const string globalThrottleKey = "general";

		// Настройки логики выполнения
		public Dictionary<RoleLimits, ThrottleOptionsBase> RoleLimits { get; set; }

		public object Handlers { get; set; }


		public ThrottleMiddleware(RequestDelegate next, IMemoryCache memoryCache, Dictionary<RoleLimits, ThrottleOptionsBase> roleLimits)
		{
			this.next = next;

			this.memoryCache = memoryCache;

			this.RoleLimits = roleLimits;
		}

		public async Task InvokeAsync(HttpContext httpContext, IUserReciever<User> userReciever)
		{
			Endpoint? endpoint = httpContext.GetEndpoint();

			if (endpoint is null)
			{
				await next.Invoke(httpContext);

				return;
			}

			ThrottleAttributeBase<ThrottleOptionsBase>? throttleAttribute = GetThrottleAttrubite(endpoint);


			string identifier = GetIdentifier(httpContext, userReciever);

			string key = (throttleAttribute?.Key) ?? globalThrottleKey;

			string entryKey = $"{identifier}:{key}";

			ThrottleOptionsBase? options = throttleAttribute?.GetOptions();

			bool isThrottle = false;


			ThrottleHandlerBase<ThrottleOptionsBase> throttleHandler = GetThrottleHandlerByOptions(options);

			isThrottle = throttleHandler.Handle(entryKey, options);


			if (isThrottle)
			{
				await SendError(httpContext.Response, key);

				return;
			}

			await next.Invoke(httpContext);
		}

		private string GetIdentifier(HttpContext httpContext, IUserReciever<User> userReciever)
		{
			User? user = userReciever.Get();

			if (user is not null)
			{
				return user.Id.ToString();
			}

			return httpContext.Request.Host.Host;
		}

		private ThrottleHandlerBase<ThrottleOptionsBase> GetThrottleHandlerByOptions(ThrottleOptionsBase options)
		{
			foreach (ThrottleHandlerBase<ThrottleOptionsBase> handler in Handlers)
			{

				if (handler.GetType().GetGenericArguments()[0] == options.GetType())
				{
					return handler;
				}
			}

			throw new Exception("Необходимый throttle обработчик не найден");
		}

		private ThrottleAttributeBase<ThrottleOptionsBase>? GetThrottleAttrubite(Endpoint endpoint)
		{
			ControllerActionDescriptor endpointController = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>()!;

			TypeInfo typeController = endpointController.ControllerTypeInfo;

			MethodInfo typeMethod = endpointController.MethodInfo;


			ThrottleAttributeBase<ThrottleOptionsBase>? throttleController = typeController.GetCustomAttribute<ThrottleAttributeBase<ThrottleOptionsBase>>();

			ThrottleAttributeBase<ThrottleOptionsBase>? throttleMethod = typeMethod.GetCustomAttribute<ThrottleAttributeBase<ThrottleOptionsBase>>();

			return throttleMethod ?? throttleController;
		}

		private async Task SendError(HttpResponse httpResponse, string overloadEndpoint)
		{
			string details = $"Too much request to : {overloadEndpoint}";

			TooManyRequestsException exception = new TooManyRequestsException("You're sending too much requests", details);

			ErrorDetails errorDetails = new ErrorDetails(exception.GetType().Name, exception.Title, exception.Details);

			await httpResponse.WriteAsync(JsonSerializer.Serialize(errorDetails));
		}
	}

	public static class ThrottleMiddlewareExtension
	{
		public static void UseThrottleMiddleware(this IApplicationBuilder app, Dictionary<RoleLimits, ThrottleOptionsBase> roleLimits)
		{
			app.UseMiddleware<ThrottleMiddleware>(roleLimits);
		}
	}
}
