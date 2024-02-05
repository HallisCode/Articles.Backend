using AspNet.Throttle.Options;
using Microsoft.Extensions.Caching.Memory;

namespace AspNet.Throttle.Handlers
{
	public class ThrottleRestingHandler : ThrottleHandlerBase<ThrottleRestingOptions>
	{
		public ThrottleRestingHandler(IMemoryCache memoryCache) : base(memoryCache)
		{
		}

		protected override LimitingRestingContext CreateEntry(string key, ThrottleRestingOptions options)
		{
			LimitingRestingContext context = new LimitingRestingContext(options.TokenLimit);

			memoryCache.Set<LimitingRestingContext>(key, context, options.WindowPeriod);

			return context;
		}

		protected override void ExecuteRules(string key, LimitingContextBase context)
		{
			LimitingRestingContext _context = (LimitingRestingContext) context;
		}

		protected class LimitingRestingContext : LimitingContextBase
		{
			public LimitingRestingContext(int tokensCount) : base(tokensCount)
			{
			}
		}
	}
}
