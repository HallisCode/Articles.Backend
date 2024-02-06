using AspNet.Throttle.Options;

namespace AspNet.Throttle
{
    public delegate bool ThrottleDelegate<in TOptions>(string key, TOptions options) where TOptions : IThrottleOptions;
}
