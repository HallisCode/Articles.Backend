using AspNet.Throttle.Options;
using System;

namespace AspNet.Throttle.Attrubites
{
	public class RataLimitWindowAttribute : RateLimitAttributeBase
	{
		public TimeSpan WindowPeriod { get; private set; }

		public RataLimitWindowAttribute(string key, int tokenLimit, double windowPeriod) : base(key, tokenLimit)
		{
			WindowPeriod = TimeSpan.FromSeconds(windowPeriod);
		}

		public override ThrottleWindowOptions GetOptions()
		{
			return new ThrottleWindowOptions(TokenLimit, WindowPeriod.TotalSeconds);
		}
	}
}
