using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models.DTOs.RequestDTOs;
using Domain.Models.Entities;
using Microsoft.Extensions.Configuration;
using System.Text.Json;


namespace Application.Services
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
            int expirationPeriod = CalculateExpirationPeriod(newItem.ExpirationPeriodInSeconds);
            var existingItem = await _repository.GetItemByKeyAsync(newItem.Key);

            var entity = new Item
            {
                Key = newItem.Key,
                Content = JsonSerializer.Serialize(newItem.Content),
                ExpirationPeriod = expirationPeriod,
                ExpiresAt = DateTime.UtcNow.AddSeconds(expirationPeriod),
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
                    await _repository.DeleteItemByKeyAsync(item.Key);
            }
        }

        public async Task Append(ItemAppend itemDto)
        {
            var existingItem = await _repository.GetItemByKeyAsync(itemDto.Key!);
            int defaultExpirationInSeconds = _configuration.GetValue<int>("DefaultValues:DefaultExpirationValue");
            if (existingItem != null)
            {
                var contentList = JsonSerializer.Deserialize<List<object>>(existingItem.Content) ?? [];

                contentList.Add(itemDto.ContentToAppend!);

                existingItem.Content = JsonSerializer.Serialize(contentList);

                existingItem.ExpiresAt = DateTime.UtcNow.AddSeconds(existingItem.ExpirationPeriod ?? defaultExpirationInSeconds);

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
                    ExpirationPeriod = defaultExpirationInSeconds,
                    ExpiresAt = DateTime.UtcNow.AddSeconds(defaultExpirationInSeconds)
                };

                await _repository.InsertItemAsync(newItem!);
            }
        }

        public async Task<List<object>?> GetItemByKeyAsync(string key)
        {
            var item = await _repository.GetItemByKeyAsync(key) ?? throw new NotFoundException();

            item.ExpiresAt = DateTime.UtcNow.AddSeconds(Convert.ToDouble(item.ExpirationPeriod));
            await _repository.UpdateItemAsync(item);

            var content = JsonSerializer.Deserialize<List<object>>(item.Content);
            return content;
        }

        private int CalculateExpirationPeriod(int? expirationPeriod)
        {
            int defaultExpirationPeriod = _configuration.GetValue<int>("DefaultValues:DefaultExpirationValue");
            if (expirationPeriod.HasValue && expirationPeriod <= defaultExpirationPeriod)
            {
                return expirationPeriod.Value;
            }
            else
            {
                return defaultExpirationPeriod;
            }
        }

        public async Task DeleteItemByKeyAsync(string key)
        {
            _ = await _repository.GetItemByKeyAsync(key) ?? throw new NotFoundException();

            await _repository.DeleteItemByKeyAsync(key);
        }
    }
}