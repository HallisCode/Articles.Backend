using AspNet.Throttle.Options;
using System;

namespace AspNet.Throttle.Attrubites
{
	public class ThrottleSlidingWindowAttribute : ThrottleAttributeBase<ThrottleSlidingWindowOptions>
	{
		public TimeSpan Window { get; private set; }

		public int Segments { get; private set; }

		public ThrottleSlidingWindowAttribute(string key, int tokenLimit, int segmentPerWindow, double window) : base(key, tokenLimit)
		{
			Window = TimeSpan.FromSeconds(window);

			Segments = segmentPerWindow;
		}

		public override ThrottleSlidingWindowOptions GetOptions()
		{
			return new ThrottleSlidingWindowOptions(TokenLimit, Segments, Window.TotalSeconds);
		}
	}
}
                                                                                                                                                                                                                                                                                                                        