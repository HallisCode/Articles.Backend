using System;

namespace AspNet.Throttle.Options
{
	public class ThrottleSlidingWindowOptions : IThrottleOptions
	{
		public int TokenLimit { get; private set; }

		public TimeSpan TimeInterval { get; private set; }

		public int SegmentsCount { get; private set; }

		public TimeSpan Interval { get; private set; }


		public ThrottleSlidingWindowOptions(int tokenLimit, int segmentsCount, double timeIntervalSeconds)
		{
			this.TokenLimit = tokenLimit;

			this.SegmentsCount = segmentsCount;

			this.TimeInterval = TimeSpan.FromSeconds(timeIntervalSeconds);

			this.Interval = TimeInterval / segmentsCount;
		}
	}
}
