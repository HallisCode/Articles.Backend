using System;

namespace AspNet.Throttle.Options
{
	public class ThrottleRestingOptions : IThrottleOptions
	{
		public int TokenLimit { get; private set; }

		public TimeSpan TimeInterval { get; private set; }


		public ThrottleRestingOptions(int tokenLimit, double timeIntervalSeconds)
		{
			this.TokenLimit = tokenLimit;

			this.TimeInterval = TimeSpan.FromSeconds(timeIntervalSeconds);
		}
	}
}
