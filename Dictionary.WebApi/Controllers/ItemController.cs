using Domain.Interfaces;
using Domain.Models.DTOs.RequestDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Dictionary.WebApi.Controllers
{
    [ApiController]
    [Route("item")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        /// <summary>
        /// Adds new item
        /// </summary>     
        /// <remarks>
        /// Adding item without expiration period sets it to default
        /// </remarks>
        /// <param name="newItem">New item object that needs to be added to the database</param>
        /// <returns>The retrieved item</returns>
        /// <response code="201">Item was successfully created</response>
        /// <response code="400">Invalid item</response>
        /// <response code="500">Server error</response>
        [HttpPost]
        public async Task<IActionResult> Create(ItemRequest newItem)
        {
            await _itemService.Create(newItem);
            return CreatedAtAction(nameof(GetItemByKey), new { newItem.Key }, newItem);
        }

        /// <summary>
        /// Gets an item 
        /// </summary>
        /// <remarks>
        /// Getting item by key updates its expiration date according to expiration period assigned to an item.
        /// </remarks>
        /// <param name="key">The key to retrieve the an item.</param>
        /// <response code="200">Returns the item if it exists</response>
        /// <response code="404">Item was not found</response>
        /// <response code="400">Invalid item</response>
        /// <response code="500">Server error</response>
        [HttpGet("{key}")]
        public async Task<IActionResult> GetItemByKey(string key)
        {
            return Ok(await _itemService.GetItemByKeyAsync(key));
        }

        /// <summary>
        /// Updates existing item 
        /// </summary>        
        /// <remarks>
        /// If item is not found a new item will be created.
        /// </remarks>
        /// <param name="itemDto">Item object that needs to be appended to the database.</param>
        /// <response code="204">Returns nothing if append was successful</response>
        /// <response code="400">Invalid item</response>
        /// <response code="500">Server error</response>
        [HttpPost("append")]
        public async Task<IActionResult> Append(ItemAppend itemDto)
        {
            await _itemService.Append(itemDto);
            return NoContent();
        }

        /// <summary>
        /// Deletes item
        /// </summary>
        /// <param name="key">The key to check if an item to delete exists.</param>
        /// <response code="204">Returns nothing if append was successful</response>
        /// <response code="404">Item was not found</response>
        /// <response code="400">Invalid item</response>
        /// <response code="500">Server error</response>
        [HttpDelete("{key}")]
        public async Task<IActionResult> DeleteItemByKey(string key)
        {
            await _itemService.DeleteItemByKeyAsync(key);
            return NoContent();
        }
    }
}