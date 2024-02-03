using AspNet.Dto;
using AspNet.Throttle.Attrubites;
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
using System.Text.Json;
using System.Threading.Tasks;

namespace AspNet.Throttle.Middlewares
{
	/// <summary>
	/// <para>Миддлварь для контроля за количеством запросов во времени к серверу (request/time).</para>
	/// <para>Задаёт ограничения на основе параметров, пока не попадутся заданные значения, по схеме : controller attribute -> class attribute-> general settings</para>
	/// </summary>
	public sealed class ThrottleMiddleware
    {
        private readonly RequestDelegate next;

        private readonly IMemoryCache memoryCache;

        private readonly Dictionary<RoleLimits, ThrottleMiddlewareOptions> roleLimits;


        private const string globalThrottleKey = "general";


        public ThrottleMiddleware(RequestDelegate next, IMemoryCache memoryCache, Dictionary<RoleLimits, ThrottleMiddlewareOptions> roleLimits)
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
			RateLimitAttribute? throttleOwner =  typeOwnerController.GetCustomAttribute<RateLimitAttribute>();
            // Настройки throttle от метода
            RateLimitAttribute? throttleController = typeController.GetCustomAttribute<RateLimitAttribute>();

            // Определяем идентификатор для ограничения пользователя
            // По умолчанию ip, в случае если пользователь аутентифицирован - его id
            string identifier = httpContext.Request.Host.ToString();

            ThrottleMiddlewareOptions options = roleLimits[RoleLimits.anonymous];

            User? user = (User?)httpContext.Items["User"];

			if (user is not null)
            {
                options = roleLimits[RoleLimits.identifier];

                identifier = user.Id.ToString();
            }

            

            string additionalKey;

            int tokenCounts;

            TimeSpan timeSpan;

            bool isRestingMode;

            RateLimitAttribute? rateLimitAttribute = GetFirstOrDefault(throttleController, throttleOwner);

			// Задаём настройки ограничения, в зависимости от того, где они указаны
			if (rateLimitAttribute is not null)
            {
                additionalKey = rateLimitAttribute.Key;

                tokenCounts = rateLimitAttribute.TokenCounts;

                timeSpan = rateLimitAttribute.TimeSpan;

                isRestingMode = rateLimitAttribute.IsRestingMode;
            }
            else
            {
				additionalKey = globalThrottleKey;

				tokenCounts = options.TokensCount;

				timeSpan = options.TimeSpan;

                isRestingMode = options.isRestingMode;
			}


			string entryKey = $"{identifier}:{additionalKey}";

            LimitingContext? limitingContext;

            bool isExistLimitingContext = memoryCache.TryGetValue<LimitingContext>(entryKey, out limitingContext);


            if (isExistLimitingContext is false)
            {
                limitingContext = new LimitingContext(tokenCounts);

                memoryCache.Set(entryKey, limitingContext, timeSpan);
			}

			if (isExistLimitingContext && limitingContext.TokensCount <= 0)
			{
				// isRestingMode true заставляет время истечения ограничения обнуляться, пока не пройдёт время ожидания целиком
				if (isRestingMode) memoryCache.Set(entryKey, limitingContext, timeSpan);

                string details = $"Too much request to : {additionalKey}";

				TooManyRequestsException exception = new TooManyRequestsException("You're sending too much requests", details);

				ErrorDetails errorDetails = new ErrorDetails(exception.GetType().Name, exception.Title, exception.Details);

				await httpContext.Response.WriteAsync(JsonSerializer.Serialize(errorDetails));

                return;
			}

			limitingContext.TokensCount--;


            await next.Invoke(httpContext);
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
    }

    public static class ThrottleMiddlewareExtension
    {
        public static void UseThrottleMiddleware(this IApplicationBuilder app, Dictionary<RoleLimits, ThrottleMiddlewareOptions> roleLimits)
        {

            app.UseMiddleware<ThrottleMiddleware>(roleLimits);
        }
    }

    /// <summary>
    /// Задаёт общие настройки для всех запросов без параметров
    /// </summary>
    public sealed class ThrottleMiddlewareOptions
    {
        public TimeSpan TimeSpan { get; private set; }

        public int TokensCount { get; private set; }
        
        public bool isRestingMode { get; private set; }

        public ThrottleMiddlewareOptions(int tokensCount, double seconds, bool isRestingMode = false)
        {
            this.TokensCount = tokensCount;

            this.TimeSpan = TimeSpan.FromSeconds(seconds);

            this.isRestingMode = isRestingMode;
        }
    }
}

