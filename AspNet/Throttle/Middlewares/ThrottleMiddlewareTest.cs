using AspNet.Dto;
using AspNet.Throttle.Attrubites;
using AspNet.Throttle.Enum;
using AspNet.Throttle.Options;
using Domain.Entities.UserScope;
using Domain.Exceptions;
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

		public ThrottleMiddlewareTest(RequestDelegate next, IMemoryCache memoryCache, Dictionary<RoleLimits, ThrottleOptionsBase> roleLimits)
		{
			this.next = next;

			this.memoryCache = memoryCache;


			if (roleLimits is null) throw new Exception("RoleLimits is null");

			this.roleLimits = roleLimits;
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


			LimitingContext? limitingContext;

			bool isExistLimitingContext = memoryCache.TryGetValue<LimitingContext>(entryKey, out limitingContext);

			if (options is ThrottleRestingOptions throttleRestingOptions)
			{
				if (isExistLimitingContext is false)
				{
					limitingContext = new LimitingContext(throttleRestingOptions.TokenLimit);

					memoryCache.Set(entryKey, limitingContext, throttleRestingOptions.WindowPeriod);
				}

				if (isExistLimitingContext && limitingContext.TokensCount <= 0)
				{
					memoryCache.Set(entryKey, limitingContext, throttleRestingOptions.WindowPeriod);

					await SendError(httpContext.Response, additionalKey);

					return;
				}
			}
			else if (options is ThrottleWindowOptions throttleWindowOptions)
			{
				if (isExistLimitingContext is false)
				{
					limitingContext = new LimitingContext(throttleWindowOptions.TokenLimit);

					memoryCache.Set(entryKey, limitingContext, throttleWindowOptions.WindowPeriod);
				}

				if (isExistLimitingContext && limitingContext.TokensCount <= 0)
				{
					await SendError(httpContext.Response, additionalKey);

					return;
				}
			}
			else if (options is ThrottleSlidingWindowOptions throttleBucketOptions)
			{
				if (isExistLimitingContext is false)
				{
					limitingContext = new LimitingContext(throttleBucketOptions.TokenLimit);

					memoryCache.Set(entryKey, limitingContext, throttleBucketOptions.Window);
				}
			}


			limitingContext.TokensCount--;

			await next.Invoke(httpContext);
		}

		private async Task SendError(HttpResponse httpResponse, string overloadEndpoint)
		{
			string details = $"Too much request to : {overloadEndpoint}";

			TooManyRequestsException exception = new TooManyRequestsException("You're sending too much requests", details);

			ErrorDetails errorDetails = new ErrorDetails(exception.GetType().Name, exception.Title, exception.Details);

			await httpResponse.WriteAsync(JsonSerializer.Serialize(errorDetails));
		}

		/// <summary>
		/// Контекст ограничения запросов
		/// </summary>
		private class LimitingContext
		{
			public int TokensCount { get; set; }

			public LimitingContext(int tokensCount)
			{
				TokensCount = tokensCount;
			}
		}

		/// <summary>
		/// Контекст ограничения запросов
		/// </summary>
		private class LimitingContextBucket
		{
			public int TokensCount { get; set; }

			public List<int> Segments { get; set; }

			public LimitingContextBucket(int tokensCount, int segments)
			{
				TokensCount = tokensCount;

				Segments = new List<int>(segments);

			}
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
}
