using AspNet.Throttle.Options;
using System;

namespace AspNet.Throttle.Attrubites
{
	public class ThrottleWindowAttribute : ThrottleAttributeBase<ThrottleWindowOptions>
	{
		public TimeSpan WindowPeriod { get; private set; }

		public ThrottleWindowAttribute(string key, int tokenLimit, double windowPeriod) : base(key, tokenLimit)
		{
			WindowPeriod = TimeSpan.FromSeconds(windowPeriod);
		}

		public override ThrottleWindowOptions GetOptions()
		{
			return new ThrottleWindowOptions(TokenLimit, WindowPeriod.TotalSeconds);
		}
	}
}
