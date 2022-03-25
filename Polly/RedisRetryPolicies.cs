using Polly;
using Polly.Retry;
using StackExchange.Redis;

namespace WebAPI.Polly
{
    public class RedisRetryPolicies
    {
        public RetryPolicy ConstDelayRetry { get; set; }

        public RedisRetryPolicies()
        {
            ConstDelayRetry = Policy.Handle<RedisConnectionException>().WaitAndRetry(5, fnc => TimeSpan.FromSeconds(2));
        }
    }
}
