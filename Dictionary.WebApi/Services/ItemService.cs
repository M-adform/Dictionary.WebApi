using Dictionary.WebApi.Models.DTOs.RequestDTOs;
using Dictionary.WebApi.Models.Entities;
//using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Dictionary.WebApi.Services
{
    public class ItemService
    {
        private readonly IConfiguration _configuration;
        public ItemService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task Create(ItemRequest newItem)
        {
            //if key exists - override value
            //if doesn't exist - create new
            //when created - return expiration period
            //when creating user inputs expiration period?
            int defaultExpirationInSeconds = _configuration.GetValue<int>("AppConfig:ExpirationPeriodInSeconds");
            int expirationPeriod;
            if(newItem.ExpirationPeriodInSeconds.HasValue && newItem.ExpirationPeriodInSeconds <= defaultExpirationInSeconds)
            {
                expirationPeriod = (int)newItem.ExpirationPeriodInSeconds;
            }
            else
            {
                expirationPeriod = defaultExpirationInSeconds;
            }
            var entity = new Item
            {
                Key = newItem.Key,
                Content = JsonConvert.SerializeObject(newItem.Content),
                ExpirationPeriod = expirationPeriod,
                ExpirationDate = DateTime.UtcNow.AddSeconds(expirationPeriod)
            };
            
            
        }
    }
}
