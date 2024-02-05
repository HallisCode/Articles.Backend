using System;

namespace AspNet.Throttle.Options
{
	public class ThrottleSlidingWindowOptions : ThrottleOptionsBase
	{
		public TimeSpan Window { get; private set; }

		public int Segments { get; private set; }


		public ThrottleSlidingWindowOptions(int tokenLimit, int segments, double window) : base(tokenLimit)
		{
			Window = TimeSpan.FromSeconds(window);

			Segments = segments;
		}
	}
}
