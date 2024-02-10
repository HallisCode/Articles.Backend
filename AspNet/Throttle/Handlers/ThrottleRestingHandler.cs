using AspNet.Throttle.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace AspNet.Throttle.Handlers
{
	public class ThrottleRestingHandler : ThrottleHandlerBase<ThrottleRestingOptions>
	{
		public ThrottleRestingHandler(IMemoryCache memoryCache) : base(memoryCache)
		{
		}

		protected override void ExecuteThrottleRules(string key, ThrottleRestingOptions options, IContext context)
		{
			if (context is not LimitingRestingContext)
			{
				throw new Exception($"Контекст типа {context.GetType()} не соотвествует ожидаемому типу {typeof(LimitingRestingContext)}");
			}

			LimitingRestingContext _context = (LimitingRestingContext)context;

			memoryCache.Set(key, _context, options.TimeInterval);
		}

		protected override LimitingRestingContext SetContext(string key, ThrottleRestingOptions options)
		{
			LimitingRestingContext context = new LimitingRestingContext(options.TokenLimit);

			memoryCache.Set(key, context, options.TimeInterval);

			return context;
		}

		protected class LimitingRestingContext : LimitingContext
		{
			public LimitingRestingContext(int tokenLimit) : base(tokenLimit)
			{
			}
		}
	}
}
