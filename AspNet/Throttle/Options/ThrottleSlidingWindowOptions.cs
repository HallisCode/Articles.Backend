using System;

namespace AspNet.Throttle.Options
{
	public class ThrottleSlidingWindowOptions : ThrottleOptionsBase
	{
		public TimeSpan Window { get; private set; }

		public int SegmentsPerWindow { get; private set; }


		public ThrottleSlidingWindowOptions(int tokenLimit, int segmentPerWindow, double window = 300d) : base(tokenLimit)
		{
			Window = TimeSpan.FromSeconds(window);

			SegmentsPerWindow = segmentPerWindow;
		}
	}
}
