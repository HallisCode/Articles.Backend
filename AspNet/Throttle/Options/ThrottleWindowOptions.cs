﻿using System;

namespace AspNet.Throttle.Options
{
	public class ThrottleWindowOptions : IThrottleOptions
	{
		public int TokenLimit { get; set; }

		public TimeSpan TimeInterval { get; set; }

		public ThrottleWindowOptions(int tokenLimit, double timeIntervalSeconds)
		{
			this.TokenLimit = tokenLimit;

			this.TimeInterval = TimeSpan.FromSeconds(timeIntervalSeconds);
		}
	}
}
