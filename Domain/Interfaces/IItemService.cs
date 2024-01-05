using Domain.Models.DTOs.RequestDTOs;

namespace Domain.Interfaces
{
    public interface IItemService
    {
        Task CleanupAsync();
        Task Create(ItemRequest newItem);
        Task Append(ItemAppend itemDto);
        Task<List<object>?> GetItemByKeyAsync(string key);
        Task DeleteItemByKeyAsync(string key);
    }
}