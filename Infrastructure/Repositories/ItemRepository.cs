using Dapper;
using Domain.Interfaces;
using Domain.Models.Entities;
using System.Data;

namespace Infrastructure.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly IDbConnection _dbConnection;

        public ItemRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<List<Item>> GetItemsAsync()
        {
            string query = @"SELECT 
                            id,
                            key,
                            content,
                            expires_at,
                            expiration_period
                            FROM items";

            var items = await _dbConnection.QueryAsync<Item>(query);
            return items.ToList();
        }

        public async Task DeleteItemByKeyAsync(string key)
        {
            string query = @"DELETE FROM items WHERE key = @key;";
            var queryArguments = new { key };

            await _dbConnection.ExecuteAsync(query, queryArguments);
        }

        public async Task<Item?> GetItemByKeyAsync(string key)
        {
            string query = @"SELECT 
                    id,
                    key,
                    content,
                    expires_at,
                    expiration_period
                    FROM items WHERE key = @key";

            var queryArguments = new { key };

            return await _dbConnection.QueryFirstOrDefaultAsync<Item>(query, queryArguments);
        }

        public async Task InsertItemAsync(Item item)
        {
            var sql = "INSERT INTO items (key, content, expiration_period, expires_at) VALUES (@Key, @Content, @ExpirationPeriod, @ExpiresAt)";
            var queryArguments = new { item.Content, item.ExpirationPeriod, item.ExpiresAt, item.Key };
            await _dbConnection.ExecuteAsync(sql, queryArguments);
        }
        public async Task CreateItemAsync(Item item)
        {
            string query = @"INSERT INTO items (key, content, expires_at, expiration_period)
                            VALUES (@Key, @Content, @ExpiresAt, @ExpirationPeriod)";
            await _dbConnection.ExecuteAsync(query, new
            { item.Id, item.Key, item.Content, item.ExpirationPeriod, item.ExpiresAt });
        }

        public async Task UpdateItemAsync(Item item)
        {
            var query = @"UPDATE items 
                        SET 
                        content = @Content, 
                        expiration_period = @ExpirationPeriod, 
                        expires_at = @ExpiresAt 
                        WHERE key = @Key";

            var queryArguments = new { item.Content, item.ExpirationPeriod, item.ExpiresAt, item.Key };
            await _dbConnection.ExecuteAsync(query, new
            { item.Id, item.Key, item.Content, item.ExpirationPeriod, item.ExpiresAt });
        }

        public async Task OverrideContentValue(Item item)
        {
            string query = @"UPDATE items SET content = @Content WHERE key = @Key";
            var queryArguments = new { item.Content, item.Key };
            await _dbConnection.ExecuteAsync(query, queryArguments);
        }
    }
}
