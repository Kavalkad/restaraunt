using System.ComponentModel.DataAnnotations;

namespace restaraunt.Core.DTO
{
    public class CreateOrderDto
    {
        [Required]
        [Range(1, 100)]
        public int TableNumber { get; set; }
        
        public string? CustomerName { get; set; }
        
        [Required]
        [MinLength(1, ErrorMessage = "Order must contain at least one item")]
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        
        public string? Notes { get; set; }
        
        public int? EstimatedPrepTime { get; set; }
    }
    
    public class OrderItemDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; } = 1;
        
        [Required]
        [Range(0.01, 10000)]
        public decimal Price { get; set; }
        
        public string? Notes { get; set; }
    }
}