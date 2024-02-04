using AspNet.Dto;
using AspNet.Throttle.Attrubites;
using AspNet.Throttle.Enum;
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
    /// <summary>
    /// <para>Миддлварь для контроля за количеством запросов во времени к серверу (request/time).</para>
    /// <para>Задаёт ограничения на основе параметров, пока не попадутся заданные значения, по схеме : controller attribute -> class attribute-> general settings</para>
    /// </summary>
    public sealed class ThrottleMiddleware
    {
        private readonly RequestDelegate next;

        private readonly IMemoryCache memoryCache;

        private readonly Dictionary<RoleLimits, ThrottleWindowOptions> roleLimits;


        private const string globalThrottleKey = "general";


        public ThrottleMiddleware(RequestDelegate next, IMemoryCache memoryCache, Dictionary<RoleLimits, ThrottleWindowOptions> roleLimits)
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
			RateLimitAttributeBase? throttleOwner =  typeOwnerController.GetCustomAttribute<RateLimitAttributeBase>();
            // Настройки throttle от метода
            RateLimitAttributeBase? throttleController = typeController.GetCustomAttribute<RateLimitAttributeBase>();

            // Определяем идентификатор для ограничения пользователя
            // По умолчанию ip, в случае если пользователь аутентифицирован - его id
            string identifier = httpContext.Request.Host.ToString();

			ThrottleWindowOptions options = roleLimits[RoleLimits.anonymous];

            User? user = (User?)httpContext.Items["User"];

			if (user is not null)
            {
                options = roleLimits[RoleLimits.identifier];

                identifier = user.Id.ToString();
            }
            
            
            string additionalKey = globalThrottleKey;

			int tokenCounts = options.TokenLimit;

			TimeSpan timeSpan = options.WindowPeriod;


			RateLimitAttributeBase? rateLimitAttribute = GetFirstOrDefault(throttleController, throttleOwner);

			// Задаём настройки, если есть атрибут ограничения
			if (rateLimitAttribute is not null)
            {
                additionalKey = rateLimitAttribute.Key;

                tokenCounts = rateLimitAttribute.TokenCounts;

                timeSpan = rateLimitAttribute.TimeSpan;

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
        public static void UseThrottleMiddleware(this IApplicationBuilder app, Dictionary<RoleLimits, ThrottleOptionsBase> roleLimits)
        {

            app.UseMiddleware<ThrottleMiddleware>(roleLimits);
        }
    }

}

