using Dictionary.WebApi.Interfaces;
using Dictionary.WebApi.Models.DTOs.RequestDTOs;
using Dictionary.WebApi.Models.Entities;
using ItemStore.WebApi.csproj.Exceptions;
using System.Text.Json;
using Dictionary.WebApi.Repositories;
using Dictionary.WebApi.Helpers;


namespace Dictionary.WebApi.Services
{
    public class ItemService : IItemService
    {
        private readonly IConfiguration _configuration;
        private readonly IItemRepository _repository;
        private readonly UpdateExpiresAtAndExpirationPeriod _updateExpiration;

        public ItemService(IConfiguration configuration, IItemRepository repository, UpdateExpiresAtAndExpirationPeriod updateExpiration)
        {
            _configuration = configuration;
            _repository = repository;
            _updateExpiration = updateExpiration;

        }

        public async Task Create(ItemRequest newItem)
        {
            int expirationPeriod = _updateExpiration.GetValidExpirationPeriod(_configuration, newItem.ExpirationPeriodInSeconds);
            var existingItem = await _repository.GetItemByKeyAsync(newItem.Key);

            var entity = new Item
            {
                Key = newItem.Key,
                Content = JsonSerializer.Serialize(newItem.Content),
                ExpirationPeriod = expirationPeriod,
                ExpiresAt = _updateExpiration.UpdateExpirationTime(expirationPeriod)
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

        public async Task AppendItem(AppendItem itemDto)
        {
            var existingItem = await _repository.GetItemByKeyAsync(itemDto.Key!);
            int defaultExpirationInSeconds = _configuration.GetValue<int>("DefaultValues:DefaultExpirationValue");
            if (existingItem != null)
            {
                var contentList = JsonSerializer.Deserialize<List<object>>(existingItem.Content) ?? new List<object>();

                contentList.Add(itemDto.ContentToAppend!);

                existingItem.Content = JsonSerializer.Serialize(contentList);


                existingItem.ExpirationPeriod = itemDto.ExpirationPeriod ?? defaultExpirationInSeconds;
                existingItem.ExpiresAt = DateTime.UtcNow.AddSeconds((double)itemDto.ExpirationPeriod!);

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
                    ExpirationPeriod = itemDto.ExpirationPeriod ?? defaultExpirationInSeconds,
                    ExpiresAt = DateTime.UtcNow.AddSeconds((double)itemDto.ExpirationPeriod!)
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

    }
}