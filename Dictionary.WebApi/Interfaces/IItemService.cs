using Dictionary.WebApi.Models.DTOs.RequestDTOs;

namespace Dictionary.WebApi.Interfaces
{
    public interface IItemService
    {
        Task CleanupAsync();
        Task Create(ItemRequest newItem);
    }
}