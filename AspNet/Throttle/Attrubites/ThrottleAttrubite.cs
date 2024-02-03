using System;

namespace AspNet.Throttle.Attrubites
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class ThrottleAttrubite : Attribute
	{
		public string Key { get; private set; }

		public int TokenCounts { get; private set; }

		public TimeSpan TimeSpan { get; private set; }

		public ThrottleAttrubite(int tokenCounts, TimeSpan timeSpan, string key)
		{
			this.Key = key;

			this.TokenCounts = tokenCounts;

			this.TimeSpan = timeSpan;
		}
	}
}
