﻿using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DTOs.RequestDTOs
{
    public class ItemRequest
    {
        [Required(ErrorMessage = "Key is required")]
        public string Key { get; set; }
        public List<object>? Content { get; set; }

        [System.ComponentModel.DefaultValue(null)]
        public int? ExpirationPeriodInSeconds { get; set; }
    }
}
