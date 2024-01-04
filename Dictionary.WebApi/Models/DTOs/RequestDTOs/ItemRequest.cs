using System.ComponentModel.DataAnnotations;

namespace Dictionary.WebApi.Models.DTOs.RequestDTOs
{
    public class ItemRequest
    {
        [Required(ErrorMessage = "Key is required")]
        public string Key { get; set; }
        public List<object>? Content { get; set; }
        public int? ExpirationPeriodInSeconds { get; set; }
    }
}
