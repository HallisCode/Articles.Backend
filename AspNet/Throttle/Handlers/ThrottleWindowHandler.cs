using AspNet.Throttle.Options;
using Microsoft.Extensions.Caching.Memory;

namespace AspNet.Throttle.Handlers
{
	public class ThrottleWindowHandler : ThrottleHandlerBase<ThrottleWindowOptions>
	{
		public ThrottleWindowHandler(IMemoryCache memoryCache) : base(memoryCache)
		{
		}

		protected override void ExecuteThrottleRules(ThrottleWindowOptions options, LimitingContext context)
		{

		}

		protected override LimitingWindowContext SetContext(string key, ThrottleWindowOptions options)
		{
			LimitingWindowContext context = new LimitingWindowContext(options.TokenLimit);

			memoryCache.Set(key, options, options.TimeInterval);

			return context;
		}

		protected class LimitingWindowContext : LimitingContext
		{
			public LimitingWindowContext(int tokenLimit) : base(tokenLimit)
			{
			}
		}
	}
}
