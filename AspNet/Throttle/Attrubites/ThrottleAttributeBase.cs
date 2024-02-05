using AspNet.Throttle.Enum;
using AspNet.Throttle.Options;
using System;

namespace AspNet.Throttle.Attrubites
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public abstract class ThrottleAttributeBase<T> : Attribute, IThrottleOptions<T> where T : ThrottleOptionsBase
	{
		public string Key { get; private set; }

		public int TokenLimit { get; private set; }


		public ThrottleAttributeBase(string key, int tokenLimit)
		{
			this.Key = key;

			this.TokenLimit = tokenLimit;

		}

		public abstract T GetOptions();
	}
}
