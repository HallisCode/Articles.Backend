using System;

namespace AspNet.Throttle.Attrubites
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class RateLimitAttribute : Attribute
	{
		public string Key { get; private set; }

		public int TokenCounts { get; private set; }

		public TimeSpan TimeSpan { get; private set; }

		public RateLimitAttribute(string key, int tokenCounts, double seconds)
		{
			this.Key = key;

			this.TokenCounts = tokenCounts;

			this.TimeSpan = TimeSpan.FromSeconds(seconds);
		}
	}
}
