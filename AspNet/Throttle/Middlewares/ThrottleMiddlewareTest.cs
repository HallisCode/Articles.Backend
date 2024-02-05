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
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace AspNet.Throttle.Middlewares
{
	public class ThrottleMiddlewareTest
	{
		private readonly RequestDelegate next;

		private readonly IMemoryCache memoryCache;

		private readonly Dictionary<RoleLimits, ThrottleOptionsBase> roleLimits;

		private const string globalThrottleKey = "general";


		private readonly ThrottleWindowHandler throttleWindowHandler;

		public ThrottleMiddlewareTest(RequestDelegate next, IMemoryCache memoryCache, Dictionary<RoleLimits, ThrottleOptionsBase> roleLimits)
		{
			this.next = next;

			this.memoryCache = memoryCache;


			if (roleLimits is null) throw new Exception("RoleLimits is null");

			this.roleLimits = roleLimits;


			throttleWindowHandler = new ThrottleWindowHandler(memoryCache);
		}

		public async Task InvokeAsync(HttpContext httpContext)
		{
			Endpoint? endpoint = httpContext.GetEndpoint();

			if (endpoint is null)
			{
				await next.Invoke(httpContext);

				return;
			}


			ControllerActionDescriptor endpointController = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>()!;

			TypeInfo typeOwnerController = endpointController.ControllerTypeInfo;

			MethodInfo typeController = endpointController.MethodInfo;

			// Настройки throttle от класса, в котором определенн метод
			RateLimitAttributeBase? throttleOwner = typeOwnerController.GetCustomAttribute<RateLimitAttributeBase>();
			// Настройки throttle от метода
			RateLimitAttributeBase? throttleController = typeController.GetCustomAttribute<RateLimitAttributeBase>();

			RateLimitAttributeBase? rateLimitAttribute = GetFirstOrDefault(throttleController, throttleOwner);

			// Определяем идентификатор для ограничения пользователя
			// По умолчанию ip, в случае если пользователь аутентифицирован - его id
			string identifier = httpContext.Request.Host.ToString();

			ThrottleOptionsBase options = roleLimits[RoleLimits.anonymous];

			User? user = (User?)httpContext.Items["User"];

			if (user is not null)
			{
				options = roleLimits[RoleLimits.identifier];

				identifier = user.Id.ToString();
			}

			string additionalKey = globalThrottleKey;

			if (rateLimitAttribute is not null)
			{
				options = rateLimitAttribute.GetOptions();

				additionalKey = rateLimitAttribute.Key;
			}

			string entryKey = $"{identifier}:{additionalKey}";

			bool isThrottle = false;

			if (options is ThrottleRestingOptions throttleRestingOptions)
			{

			}
			else if (options is ThrottleWindowOptions throttleWindowOptions)
			{
				isThrottle = throttleWindowHandler.Handle(entryKey, throttleWindowOptions);
			}
			else if (options is ThrottleSlidingWindowOptions throttleBucketOptions)
			{

			}

			if (isThrottle)
			{
				await SendError(httpContext.Response, additionalKey);

				return;
			}

			await next.Invoke(httpContext);
		}

		private async Task SendError(HttpResponse httpResponse, string overloadEndpoint)
		{
			string details = $"Too much request to : {overloadEndpoint}";

			TooManyRequestsException exception = new TooManyRequestsException("You're sending too much requests", details);

			ErrorDetails errorDetails = new ErrorDetails(exception.GetType().Name, exception.Title, exception.Details);

			await httpResponse.WriteAsync(JsonSerializer.Serialize(errorDetails));
		}

		private T? GetFirstOrDefault<T>(params T[] array)
		{
			foreach (T? obj in array)
			{
				if (obj is not null)
				{
					return obj;
				}
			}

			return default(T);
		}
	}

	public static class ThrottleMiddlewareTestExtension
	{
		public static void UseThrottleMiddlewareTest(this IApplicationBuilder app, Dictionary<RoleLimits, ThrottleOptionsBase> roleLimits)
		{

			app.UseMiddleware<ThrottleMiddlewareTest>(roleLimits);
		}
	}
}
