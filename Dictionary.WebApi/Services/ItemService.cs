using Dictionary.WebApi.Models.DTOs.RequestDTOs;
using Dictionary.WebApi.Models.Entities;
using Newtonsoft.Json;
﻿using Dictionary.WebApi.Interfaces;


namespace Dictionary.WebApi.Services
{
    public class ItemService : IItemService
    {
        private readonly IConfiguration _configuration;
        private readonly IItemRepository _repository;
        public ItemService(IConfiguration configuration, IItemRepository repository)
        {
            _configuration = configuration;
            _repository = repository;
            
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
            
        public async Task CleanupAsync()
        {
            var items = await _repository.GetItemsAsync();

            foreach (var item in items)
            {
                if (item.ExpiresAt < DateTime.Now)
                    await _repository.DeleteItemsAsync(item.Id);
            }
        }
    }
}