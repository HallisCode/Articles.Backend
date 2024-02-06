using System;

namespace AspNet.Throttle.Options
{
	public class ThrottleSlidingWindowOptions : ThrottleOptionsBase
	{
		public TimeSpan WindowPeriod { get; private set; }

		public int Segments { get; private set; }


		public ThrottleSlidingWindowOptions(int tokenLimit, int segments, double windowPeriod) : base(tokenLimit)
		{
			WindowPeriod = TimeSpan.FromSeconds(windowPeriod);

			Segments = segments;
		}
	}
}
