using Dictionary.WebApi.Interfaces;
using Dictionary.WebApi.Models.DTOs.RequestDTOs;
using Dictionary.WebApi.Models.Entities;
using Dictionary.WebApi.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using FluentAssertions;


namespace Dictionary.UnitTests.Services
{
    public class ItemServiceTests
    {
        private readonly Mock<IItemRepository> _mockRepository;
        private readonly IConfiguration _configuration;
        private readonly ItemService _service;

        public ItemServiceTests()
        {
            _mockRepository = new Mock<IItemRepository>();

            var inMemorySettings = new Dictionary<string, string> {
            {"DefaultValues:DefaultExpirationValue", "3600"}
        };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _service = new ItemService(_configuration, _mockRepository.Object);
        }

        [Fact]
        public async Task Create_WhenNewItem_ShouldCreateNewItemAsync()
        {
            var newItem = new ItemRequest { Key = "newKey", Content = new List<object> { new object() }, ExpirationPeriodInSeconds = 3600 };

            _mockRepository.Setup(x => x.GetItemByKeyAsync(newItem.Key)).ReturnsAsync((Item)null);

            await _service.Create(newItem);

            _mockRepository.Verify(x => x.CreateItemAsync(It.IsAny<Item>()), Times.Once);
        }

        [Fact]
        public async Task Create_WhenExistingItem_ShouldOverrideItemAsync()
        {
            var newItem = new ItemRequest { Key = "newKey", Content = new List<object> { new object() }, ExpirationPeriodInSeconds = 3600 };
            var existingItem = new Item { /* properties set up */ };

            _mockRepository.Setup(x => x.GetItemByKeyAsync(newItem.Key)).ReturnsAsync(existingItem);

            await _service.Create(newItem);

            _mockRepository.Verify(x => x.OverrideContentValue(It.IsAny<Item>()), Times.Once);
        }


        [Fact]
        public async Task DeleteItemByKeyAsync_ExistingItem_ShouldDeleteItem()
        {
            var item = new Item { Key = "key", Content = "content" };

            _mockRepository.Setup(x => x.GetItemByKeyAsync("key")).ReturnsAsync(item);

            await _service.DeleteItemByKeyAsync("key");

            _mockRepository.Verify(x => x.DeleteItemByKeyAsync("key"), Times.Once);
        }

        [Fact]
        public async Task Append_ExistingItem_CallsUpdateItemAsync()
        {
            var existingContentJson = JsonSerializer.Serialize(new List<object> { "existingContent" });
            var existingItem = new Item { Key = "existingKey", Content = existingContentJson };
            var itemAppend = new ItemAppend { Key = "existingKey", ContentToAppend = "newContent" };

            _mockRepository.Setup(x => x.GetItemByKeyAsync("existingKey")).ReturnsAsync(existingItem);

            await _service.Append(itemAppend);

            _mockRepository.Verify(x => x.UpdateItemAsync(It.Is<Item>(item => item.Key == "existingKey")), Times.Once);
        }
    }
}
