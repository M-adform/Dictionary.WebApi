using Dictionary.WebApi.Models.DTOs.RequestDTOs;
using Dictionary.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dictionary.WebApi.Controllers
{
    [ApiController]
    [Route("item")]
    public class ItemController : ControllerBase
    {
        private readonly ItemService _itemService;
        public ItemController(ItemService itemService)
        {
            _itemService = itemService;
        }
        [HttpPost]
        public async Task<IActionResult> Create(ItemRequest newItem)
        {
            await _itemService.Create(newItem);
            return Ok();
        }
    }
}
