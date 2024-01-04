using Dictionary.WebApi.Models.DTOs.RequestDTOs;
using Dictionary.WebApi.Models.Entities;
﻿using Dictionary.WebApi.Interfaces;
using System.Text.Json;


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
            int defaultExpirationInSeconds = _configuration.GetValue<int>("DefaultValues:DefaultExpirationValue");
            int expirationPeriod;
            if (newItem.ExpirationPeriodInSeconds.HasValue && newItem.ExpirationPeriodInSeconds <= defaultExpirationInSeconds)
            {
                expirationPeriod = (int)newItem.ExpirationPeriodInSeconds;
            }
            else
            {
                expirationPeriod = defaultExpirationInSeconds;
            }
            var existingItem = await _repository.GetItemByKeyAsync(newItem.Key);
            var entity = new Item
            {
                Key = newItem.Key,
                Content = JsonSerializer.Serialize(newItem.Content),
                ExpirationPeriod = expirationPeriod,
                ExpiresAt = DateTime.UtcNow.AddSeconds(expirationPeriod)
            };
            if (existingItem is null)
            {
                await _repository.CreateItemAsync(entity);
            }
            else
            {
                await _repository.OverrideContentValue(entity);
            }


        }
            
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