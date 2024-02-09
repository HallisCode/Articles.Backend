using AspNet.Throttle.Options;
using System;

namespace AspNet.Throttle.Attrubites
{
	public interface IGetThrottleOptions
	{
		public Type OptionsType { get; }

		public IThrottleOptions GetOptions();
	}
}
