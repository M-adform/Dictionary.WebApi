using Dictionary.WebApi.Models.Entities;

namespace Dictionary.WebApi.Interfaces
{
    public interface IItemRepository
    {
        Task<List<Item>> GetItemsAsync();
        Task DeleteItemsAsync(int id);
        public Task<Item?> GetItemByKeyAsync(string key);
        public Task InsertItemAsync(Item item);
        public Task UpdateItemAsync(Item item);
    }
}