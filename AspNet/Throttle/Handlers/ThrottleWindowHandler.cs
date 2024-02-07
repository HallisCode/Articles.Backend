using AspNet.Throttle.Options;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace AspNet.Throttle.Handlers
{
	public sealed class ThrottleWindowHandler : IThrottleHandler
	{
		public readonly Type OptionsType = typeof(ThrottleWindowOptions);


		private readonly IMemoryCache memoryCache;

		public ThrottleWindowHandler(IMemoryCache memoryCache)
		{
			this.memoryCache = memoryCache;
		}

		// Обязательства IThrottleHandler
		public bool Throttle(string key, IThrottleOptions options)
		{
			// Больное место моей недо-архитектуры всего модуля Throttle.
			// Конкретной реализации throttle обработчика, нужны конкретные options.
			VerifyOptionsType(options);

			// Опять приводим к конкретномку настроек, нужному для данного обработчика.
			// Данный вопрос легко решается реализацией IThrottle<TOptions> => нельзя будет все обработчики положить в одну коллекцию.
			ThrottleWindowOptions _options = (ThrottleWindowOptions)options;

			bool isExistContext = memoryCache.TryGetValue(key, out LimitingContext? context);

			if (!isExistContext)
			{
				context = SetContext(key, options);
			}

			if (isExistContext && context?.TokensAvailable <= 0)
			{
				ExecuteThrottleRules(options, context);

				return true;
			}

			context!.TokensAvailable--;

			return false;
		}

		private void ExecuteThrottleRules(IThrottleOptions options, LimitingContext context)
		{
			// Опять приводим к конкретномку настроек, нужному для данного обработчика.
			ThrottleWindowOptions _options = (ThrottleWindowOptions)options;
		}

		private LimitingContext SetContext(string key, IThrottleOptions options)
		{
			// Опять приводим к конкретномку настроек, нужному для данного обработчика.
			ThrottleWindowOptions _options = (ThrottleWindowOptions)options;


			LimitingContext context = new LimitingContext(_options.TokenLimit);

			memoryCache.Set(key, context, _options.TimeInterval);

			return context;
		}

		// Подобных штук вообще не должно существовать по сути. Еще один звоночек, что я делаю что-то не так.
		// К сожалению я не могу придумать другую архитектуру для throttle.
		private void VerifyOptionsType(object options)
		{
			if (options.GetType() != OptionsType)
			{
				throw new Exception($"Для метода {nameof(VerifyOptionsType)} класса {this.GetType().Name} необходим " +
					$"параметр типа {options.GetType()}, а не {options.GetType()}");
			}
		}

		private class LimitingContext
		{
			public int TokensAvailable { get; set; }

			public LimitingContext(int tokenLimit)
			{
				this.TokensAvailable = tokenLimit;
			}
		}

	}
}
