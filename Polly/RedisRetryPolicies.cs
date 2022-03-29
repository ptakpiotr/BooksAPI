using Npgsql;
using Polly;
using Polly.Retry;
using StackExchange.Redis;

namespace WebAPI.Polly
{
    public class RedisRetryPolicies
    {
        public RetryPolicy ConstDelayRetry { get; set; }
        public RetryPolicy ExpDelayHangRetry{ get; set; }

        public RedisRetryPolicies()
        {
            ConstDelayRetry = Policy.Handle<RedisConnectionException>().WaitAndRetry(5, fnc => TimeSpan.FromSeconds(2));
            ExpDelayHangRetry = Policy.Handle<NpgsqlException>().WaitAndRetry(5, fnc => TimeSpan.FromSeconds(Math.Pow(1.5, 5)));
        }
    }
}
