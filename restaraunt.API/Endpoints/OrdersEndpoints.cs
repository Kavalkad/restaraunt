using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restaraunt.Core.Entities;
using System;
using restaraunt.Persistence;
using restaraunt.Core.Enums;
using restaraunt.Core.DTO;

namespace restaraunt.API.Controlers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        // Временное хранилище (позже замените на базу данных)
        private static List<OrderEntity> _orders = new List<OrderEntity>();
        private static int _nextId = 1;

        private readonly ILogger<OrderController> _logger;
        private readonly AppDbContext _context;

        public OrderController(ILogger<OrderController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;

            // Добавим тестовые данные при первом запуске
            if (_orders.Count == 0)
            {
                InitializeSampleData();
            }
        }

        // GET: api/order
        [HttpGet]
        public IActionResult GetAllOrders()
        {
            _logger.LogInformation("Getting all orders");
            return Ok(_orders.OrderByDescending(o => o.CreatedAt));
        }

        // GET: api/order/active
        [HttpGet("active")]
        public IActionResult GetActiveOrders()
        {
            var activeStatuses = new[] { OrderStatus.Pending, OrderStatus.Confirmed, OrderStatus.Preparing };
            var activeOrders = _orders
                .Where(o => activeStatuses.Contains(o.Status))
                .OrderByDescending(o => o.CreatedAt)
                .ToList();

            _logger.LogInformation($"Found {activeOrders.Count} active orders");
            return Ok(activeOrders);
        }

        // GET: api/order/{id}
        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                _logger.LogWarning($"Order with id {id} not found");
                return NotFound(new { message = $"Order with id {id} not found" });
            }

            return Ok(order);
        }

        // POST: api/order
        // Actually working on this method
        [HttpPost("CreateOrder")]
        public async Task<ActionResult<OrderEntity>> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            try
            {
                _logger.LogInformation("Creating new order");

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Создаем Order из DTO
                var order = new OrderEntity
                {
                    TableNumber = orderDto.TableNumber,
                    CustomerName = orderDto.CustomerName,
                    Status = OrderStatus.Pending,
                    CreatedAt = DateTime.UtcNow,
                    Notes = orderDto.Notes,
                    EstimatedPrepTime = orderDto.EstimatedPrepTime
                };

                // Добавляем items
                foreach (var itemDto in orderDto.Items)
                {
                    order.Items.Add(new OrderItemEntity
                    {
                        Name = itemDto.Name,
                        Quantity = itemDto.Quantity,
                        Price = itemDto.Price,
                        Notes = itemDto.Notes
                    });
                }

                // Рассчитываем сумму
                order.TotalAmount = order.Items.Sum(item => item.Price * item.Quantity);

                // Сохраняем в базу
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                // // Загружаем полный объект с items
                // var createdOrder = await _context.Orders
                //     .Include(o => o.Items)
                //     .FirstOrDefaultAsync(o => o.Id == order.Id);

                _logger.LogInformation($"Order created: ID={order.Id}, Table={order.TableNumber}");

                //return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, createdOrder);
                return Ok(200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order");
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        // PUT: api/order/{id}/status
        [HttpPut("{id}/status")]
        public IActionResult UpdateOrderStatus(int id, [FromBody] UpdateStatusDto statusDto)
        {
            try
            {
                var order = _orders.FirstOrDefault(o => o.Id == id);

                if (order == null)
                {
                    return NotFound(new { message = $"Order with id {id} not found" });
                }

                if (!Enum.IsDefined(typeof(OrderStatus), statusDto.Status))
                {
                    return BadRequest(new { message = "Invalid status value" });
                }

                var oldStatus = order.Status;
                order.Status = statusDto.Status;
                order.UpdatedAt = DateTime.UtcNow;

                _logger.LogInformation($"Order {id} status changed from {oldStatus} to {order.Status}");

                return Ok(new
                {
                    message = $"Order status updated to {order.Status}",
                    order = order
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating order {id} status");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        // DELETE: api/order/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound(new { message = $"Order with id {id} not found" });
            }

            _orders.Remove(order);
            _logger.LogInformation($"Order {id} deleted");

            return Ok(new { message = $"Order {id} deleted successfully" });
        }

        // GET: api/order/table/{tableNumber}
        [HttpGet("table/{tableNumber}")]
        public IActionResult GetOrdersByTable(int tableNumber)
        {
            var tableOrders = _orders
                .Where(o => o.TableNumber == tableNumber)
                .OrderByDescending(o => o.CreatedAt)
                .ToList();

            return Ok(tableOrders);
        }

        // GET: api/order/status/{status}
        [HttpGet("status/{status}")]
        public IActionResult GetOrdersByStatus(OrderStatus status)
        {
            var ordersByStatus = _orders
                .Where(o => o.Status == status)
                .OrderByDescending(o => o.CreatedAt)
                .ToList();

            return Ok(new
            {
                status = status.ToString(),
                count = ordersByStatus.Count,
                orders = ordersByStatus
            });
        }

        private void InitializeSampleData()
        {
            var sampleOrders = new List<OrderEntity>
            {
                new OrderEntity
                {
                    Id = _nextId++,
                    TableNumber = 5,
                    CustomerName = "Иван Иванов",
                    Items = new List<OrderItemEntity>
                    {
                        new OrderItemEntity { Id = 1, Name = "Пицца Маргарита", Quantity = 1, Price = 450 },
                        new OrderItemEntity { Id = 2, Name = "Кока-Кола", Quantity = 2, Price = 120 }
                    },
                    Status = OrderStatus.Preparing,
                    CreatedAt = DateTime.UtcNow.AddMinutes(-15),
                    Notes = "Без лука",
                    EstimatedPrepTime = 20
                },
                new OrderEntity
                {
                    Id = _nextId++,
                    TableNumber = 3,
                    Items = new List<OrderItemEntity>
                    {
                        new OrderItemEntity { Id = 1, Name = "Стейк Рибай", Quantity = 1, Price = 890 },
                        new OrderItemEntity { Id = 2, Name = "Картофель фри", Quantity = 1, Price = 150 }
                    },
                    Status = OrderStatus.Pending,
                    CreatedAt = DateTime.UtcNow.AddMinutes(-5),
                    EstimatedPrepTime = 25
                },
                new OrderEntity
                {
                    Id = _nextId++,
                    TableNumber = 8,
                    CustomerName = "Анна Петрова",
                    Items = new List<OrderItemEntity>
                    {
                        new OrderItemEntity { Id = 1, Name = "Салат Цезарь", Quantity = 2, Price = 320 },
                        new OrderItemEntity { Id = 2, Name = "Суп грибной", Quantity = 1, Price = 180 },
                        new OrderItemEntity { Id = 3, Name = "Чай зеленый", Quantity = 1, Price = 100 }
                    },
                    Status = OrderStatus.Ready,
                    CreatedAt = DateTime.UtcNow.AddMinutes(-30),
                    Notes = "Салат без гренок",
                    EstimatedPrepTime = 15
                }
            };

            // Рассчитываем TotalAmount для каждого заказа
            foreach (var order in sampleOrders)
            {
                order.TotalAmount = order.Items.Sum(item => item.Price * item.Quantity);
            }

            _orders.AddRange(sampleOrders);
            _logger.LogInformation("Sample data initialized");
        }
    }

    // DTO для обновления статуса
    public class UpdateStatusDto
    {
        public OrderStatus Status { get; set; }
    }
}