using AspNet.Throttle.Options;
using System;

namespace AspNet.Throttle.Attrubites
{
	public class ThrottleSlidingWindowAttribute : Attribute, IThrottleAttribute
	{
		public Type OptionsType { get; } = typeof(ThrottleSlidingWindowOptions);

		public string Key { get; private set; }

		public int TokenLimit { get; private set; }

		public TimeSpan TimeInterval { get; private set; }

		public int SegmentsCount { get; private set; }


		public ThrottleSlidingWindowAttribute(string key, int tokenLimit, int segmentsCount, double timeIntervalSeconds)
		{
			this.Key = key;

			this.TokenLimit = tokenLimit;

			this.TimeInterval = TimeSpan.FromSeconds(timeIntervalSeconds);

			this.SegmentsCount = segmentsCount;
		}


		public IThrottleOptions GetOptions()
		{
			return new ThrottleSlidingWindowOptions(TokenLimit, SegmentsCount, TimeInterval.TotalSeconds);
		}
	}
}
