using Dictionary.WebApi.Models.Entities;

namespace Dictionary.WebApi.Interfaces
{
    public interface IItemRepository
    {
        Task<List<Item>> GetItemsAsync();
        Task DeleteItemByKeyAsync(string key);
        Task InsertItemAsync(Item item);
        Task UpdateItemAsync(Item item);
        Task CreateItemAsync(Item item);
        Task<Item?> GetItemByKeyAsync(string key);
        Task OverrideContentValue(Item item);
    }
}