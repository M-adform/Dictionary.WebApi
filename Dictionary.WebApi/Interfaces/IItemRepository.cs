using Dictionary.WebApi.Models.Entities;

namespace Dictionary.WebApi.Interfaces
{
    public interface IItemRepository
    {
        Task<List<Item>> GetItemsAsync();
        Task DeleteItemsAsync(int id);
        Task CreateItemAsync(Item item);
    }
}