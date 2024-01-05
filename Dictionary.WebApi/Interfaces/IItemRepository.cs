﻿using Dictionary.WebApi.Models.Entities;

namespace Dictionary.WebApi.Interfaces
{
    public interface IItemRepository
    {
        Task<List<Item>> GetItemsAsync();
        Task DeleteItemsAsync(int id);
        Task InsertItemAsync(Item item);
        Task UpdateItemAsync(Item item);
        Task CreateItemAsync(Item item);
        Task<Item?> GetItemByKeyAsync(string key);
        Task OverrideContentValue(Item item);
        Task<Item> GetItem(string Key);
    }
}