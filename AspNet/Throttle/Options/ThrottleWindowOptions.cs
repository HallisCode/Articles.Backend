using System;

namespace AspNet.Throttle.Options
{
	public class ThrottleWindowOptions : ThrottleOptionsBase
	{
		public TimeSpan WindowPeriod { get; private set; }

		public ThrottleWindowOptions(int tokenLimit, double windowPeriod) : base(tokenLimit)
		{
			WindowPeriod = TimeSpan.FromSeconds(windowPeriod);
		}
	}
}
