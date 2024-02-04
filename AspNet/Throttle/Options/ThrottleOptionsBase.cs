using System;

namespace AspNet.Throttle.Options
{
    public abstract class ThrottleOptionsBase
    {
        public int TokenLimit { get; private set; }


        public ThrottleOptionsBase(int tokenLimit)
        {
            TokenLimit = tokenLimit;
        }
    }
}
