using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using restaraunt.Core.Enums;

namespace restaraunt.Core.Entities
{
    public class OrderItemEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        public int Quantity { get; set; } = 1;

        [Range(0.01, 10000, ErrorMessage = "Price must be positive")]
        public decimal Price { get; set; }
        public string? Notes { get; set; }

        // Внешний ключ для Order
        public int OrderId { get; set; }

        public OrderEntity? Order { get; set; }
    }
}
