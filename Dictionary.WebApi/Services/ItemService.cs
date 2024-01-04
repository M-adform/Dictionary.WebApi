using Dictionary.WebApi.Models.DTOs.RequestDTOs;
using Dictionary.WebApi.Models.Entities;
﻿using Dictionary.WebApi.Interfaces;
using System.Text.Json;
using Dictionary.WebApi.Repositories;


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
            var entity = new Item
            {
                Key = newItem.Key,
                Content = JsonSerializer.Serialize(newItem.Content),
                ExpirationPeriod = expirationPeriod,
                ExpiresAt = DateTime.UtcNow.AddSeconds(expirationPeriod)
            };

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

        public async Task AppendItem(AppendItem itemDto)
        {
            var existingItem = await _repository.GetItemByKeyAsync(itemDto.Key!);

            if (existingItem != null)
            {
                var contentList = JsonSerializer.Deserialize<List<object>>(existingItem.Content) ?? new List<object>();

                contentList.Add(itemDto.ContentToAppend!);

                existingItem.Content = JsonSerializer.Serialize(contentList);

                existingItem.ExpirationPeriod = itemDto.ExpirationPeriod;
                existingItem.ExpiresAt = DateTime.UtcNow.AddSeconds(itemDto.ExpirationPeriod ?? 0);

                await _repository.UpdateItemAsync(existingItem!);
            }
            else
            {
                var newContentList = new List<object> { itemDto.ContentToAppend! };

                string newContent = JsonSerializer.Serialize(newContentList);

                var newItem = new Item
                {
                    Key = itemDto.Key!,
                    Content = newContent,
                    ExpirationPeriod = itemDto.ExpirationPeriod,
                    ExpiresAt = DateTime.UtcNow.AddDays(itemDto.ExpirationPeriod ?? 0)
                };

                await _repository.InsertItemAsync(newItem!);
            }
        }
    }
}