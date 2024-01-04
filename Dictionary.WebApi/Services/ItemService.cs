using Dictionary.WebApi.Interfaces;

namespace Dictionary.WebApi.Services
{
    public class ItemService : IItemService
    {
        public async Task CleanupAsync()
        {
            // Get db records, where expiration date is past NOW.
            // Remove them from database.          
        }
    }
}