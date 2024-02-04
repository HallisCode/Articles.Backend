using AspNet.Throttle.Options;
using System;

namespace AspNet.Throttle.Attrubites
{
	public class RateLimitSlidingWindowAttribute : RateLimitAttributeBase
	{
		public TimeSpan Window { get; private set; }

		public int SegmentPerWindow { get; private set; }

		public RateLimitSlidingWindowAttribute(string key, int tokenLimit, int segmentPerWindow, double window = 300d) : base(key, tokenLimit)
		{
			Window = TimeSpan.FromSeconds(window);

			SegmentPerWindow = segmentPerWindow;
		}

		public override ThrottleSlidingWindowOptions GetOptions()
		{
			return new ThrottleSlidingWindowOptions(TokenLimit, SegmentPerWindow, Window.TotalSeconds);
		}
	}
}
                                                                                                                                                                                                                                                                                                                        