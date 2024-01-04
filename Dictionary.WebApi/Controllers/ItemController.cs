using Dictionary.WebApi.Interfaces;
using Dictionary.WebApi.Models.DTOs.RequestDTOs;
using Dictionary.WebApi.Services;
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
            return Ok();
        }
        [HttpPost("append")]
        public async Task<IActionResult> Append(AppendItem itemDto)
        {
            await _itemService.AppendItem(itemDto);
            return NoContent();
        }
    }
}
