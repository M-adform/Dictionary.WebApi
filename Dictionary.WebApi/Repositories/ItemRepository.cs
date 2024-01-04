﻿using Dapper;
using Dictionary.WebApi.Interfaces;
using Dictionary.WebApi.Models.Entities;
using System.Data;

namespace Dictionary.WebApi.Repositories
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
                            value,
                            expires_at,
                            expiration_period
                            FROM items";

            var items = await _dbConnection.QueryAsync<Item>(query);
            return items.ToList();
        }

        public async Task DeleteItemsAsync(int id)
        {
            string query = @"DELETE FROM items WHERE id = @id;";
            var queryArguments = new { id };

            await _dbConnection.ExecuteAsync(query, queryArguments);
        }
    }
}
