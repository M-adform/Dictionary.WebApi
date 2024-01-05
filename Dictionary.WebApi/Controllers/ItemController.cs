using Dictionary.WebApi.Interfaces;
using Dictionary.WebApi.Models.DTOs.RequestDTOs;
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

        [HttpPost]
        public async Task<IActionResult> Create(ItemRequest newItem)
        {
            await _itemService.Create(newItem);
            return Created();
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> GetItemByKey(string key)
        {
            return Ok(await _itemService.GetItemByKeyAsync(key));
        }
        [HttpPost("append")]
        public async Task<IActionResult> Append(ItemAppend itemDto)
        {
            await _itemService.Append(itemDto);
            return NoContent();
        }
    }
}
