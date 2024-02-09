using AspNet.Throttle.Options;
using System;

namespace AspNet.Throttle.Attrubites
{
	public class ThrottleWindowAttribute : Attribute, IThrottleAttribute
	{
		public Type OptionsType { get; } = typeof(ThrottleWindowOptions);

		public string Key { get; private set; }

		public int TokenLimit { get; private set; }

		public TimeSpan TimeInterval { get; private set; }


		public ThrottleWindowAttribute(string key, int tokenLimit, double timeIntervalSeconds)
		{
			this.Key = key;

			this.TokenLimit = tokenLimit;

			this.TimeInterval = TimeSpan.FromSeconds(timeIntervalSeconds);
		}


		public IThrottleOptions GetOptions()
		{
			return new ThrottleWindowOptions(TokenLimit, TimeInterval.TotalSeconds);
		}
	}
}
