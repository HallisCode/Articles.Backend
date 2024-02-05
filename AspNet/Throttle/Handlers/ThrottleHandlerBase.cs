using AspNet.Throttle.Options;
using Microsoft.Extensions.Caching.Memory;

namespace AspNet.Throttle.Handlers
{
	public abstract class ThrottleHandlerBase<T> where T : ThrottleOptionsBase
	{
		protected readonly IMemoryCache memoryCache;

		public ThrottleHandlerBase(IMemoryCache memoryCache)
		{
			this.memoryCache = memoryCache;
		}

		/// <summary>
		/// Обрабатывает правила ограничения запросов.
		/// </summary>
		/// <returns>Истратил ли пользователь все токены.</returns>
		public virtual bool Handle(string key, T options)
		{
			LimitingContextBase? context;

			bool isExistContext = memoryCache.TryGetValue<LimitingContextBase>(key, out context);

			if (isExistContext is false)
			{
				context = CreateEntry(key, options);
			}

			if (isExistContext && context.TokensCount <= 0)
			{
				ExecuteRules(key, context);

				return true;
			}

			DecrementTokens(context);

			return false;
		}

		protected abstract LimitingContextBase CreateEntry(string key, T options);

		protected abstract void ExecuteRules(string key, LimitingContextBase context);

		protected virtual void DecrementTokens(LimitingContextBase context)
		{
			context.TokensCount--;
		}

		/// <summary>
		/// Контекст ограничения запросов
		/// </summary>
		protected abstract class LimitingContextBase
		{
			public int TokensCount { get; set; }

			public LimitingContextBase(int tokensCount)
			{
				TokensCount = tokensCount;
			}
		}
	}
}
