using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using restaraunt.Core.Enums;

namespace restaraunt.Core.Entities
{
    public class OrderEntity
    {
        public int Id { get; set; }
        
        [Range(1, 100, ErrorMessage = "Table number must be between 1 and 100")]
        public int TableNumber { get; set; }
        
        public string? CustomerName { get; set; }
        
        public List<OrderItemEntity> Items { get; set; } = new List<OrderItemEntity>();
        
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        public decimal TotalAmount { get; set; }
        
        public string? Notes { get; set; }
        
        public int? EstimatedPrepTime { get; set; }
    }

    

    
}