using Dictionary.WebApi.Models.Entities;
using Microsoft.Extensions.Configuration;

namespace Dictionary.WebApi.Helpers
{
    public class UpdateExpiresAtAndExpirationPeriod
    {
        public DateTime UpdateExpirationTime(int expirationPeriod)
        {
            return DateTime.UtcNow.AddSeconds(expirationPeriod);
        }
        public int GetValidExpirationPeriod(IConfiguration configuration, int? expirationPeriod)
        {
            int defaultExpirationPeriod = configuration.GetValue<int>("DefaultValues:DefaultExpirationValue");
            if (expirationPeriod.HasValue && expirationPeriod <= defaultExpirationPeriod)
            {
                return expirationPeriod.Value;
            }
            else
            {
                return defaultExpirationPeriod;
            }
        }
    }
}
