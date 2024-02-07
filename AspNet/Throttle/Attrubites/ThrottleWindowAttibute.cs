using AspNet.Throttle.Options;
using System;

namespace AspNet.Throttle.Attrubites
{
	public class ThrottleWindowAttibute : Attribute, IThrottleAttribute<ThrottleWindowOptions>
	{
		public string Key { get; private set; }

		public int TokenLimit { get; private set; }

		public TimeSpan TimeInterval { get; private set; }

		public ThrottleWindowAttibute(string key, int tokenLimit, double timeIntervalSeconds)
		{
			this.Key = key;

			this.TokenLimit = tokenLimit;

			this.TimeInterval = TimeSpan.FromSeconds(timeIntervalSeconds);
		}

		public ThrottleWindowOptions GetOptions()
		{
			return new ThrottleWindowOptions(TokenLimit, TimeInterval.Seconds);
		}
	}
}
