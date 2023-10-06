using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace NSE.WebAPI.Core.Polly
{
    public static class PolicyExtensions
    {
        public static AsyncRetryPolicy<HttpResponseMessage> EsperarTentar()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(new[]{
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10),
                });
        }
    }
}
