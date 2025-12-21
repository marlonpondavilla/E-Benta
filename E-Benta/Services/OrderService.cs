using E_Benta.Data;
using E_Benta.Dtos;
using E_Benta.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Benta.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        public OrderService(AppDbContext context) { _context = context; }

        public async Task<OrderResponseDto> BuyNowAsync(int userId, BuyNowRequestDto dto)
        {
            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null) throw new InvalidOperationException("Product not found");

            var order = new Order { UserId = userId, Status = "Pending" };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var item = new OrderItem
            {
                OrderId = order.Id,
                ProductId = product.Id,
                Quantity = dto.Quantity,
                UnitPrice = product.ProductPrice,
                LineTotal = product.ProductPrice * dto.Quantity
            };
            _context.OrderItems.Add(item);
            order.Total = item.LineTotal;
            await _context.SaveChangesAsync();

            order = await _context.Orders.Include(o => o.Items).ThenInclude(i => i.Product)
                .FirstAsync(o => o.Id == order.Id);
            return MapOrder(order);
        }

        public async Task<OrderResponseDto> CheckoutCartAsync(int userId)
        {
            var cart = await _context.Carts.Include(c => c.Items).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null || cart.Items.Count == 0) throw new InvalidOperationException("Cart is empty");

            var order = new Order { UserId = userId, Status = "Pending" };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var ci in cart.Items)
            {
                var item = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.UnitPrice,
                    LineTotal = ci.UnitPrice * ci.Quantity
                };
                _context.OrderItems.Add(item);
            }

            order.Total = cart.Items.Sum(x => x.UnitPrice * x.Quantity);
            await _context.SaveChangesAsync();

            // clear cart
            _context.CartItems.RemoveRange(cart.Items);
            await _context.SaveChangesAsync();

            order = await _context.Orders.Include(o => o.Items).ThenInclude(i => i.Product)
                .FirstAsync(o => o.Id == order.Id);
            return MapOrder(order);
        }

        public async Task<OrderResponseDto> CheckoutListAsync(int userId, IEnumerable<BuyNowRequestDto> items)
        {
            if (items == null || !items.Any()) throw new InvalidOperationException("No items to checkout");
            var order = new Order { UserId = userId, Status = "Pending" };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            decimal total = 0m;
            foreach (var req in items)
            {
                var product = await _context.Products.FindAsync(req.ProductId);
                if (product == null) throw new InvalidOperationException($"Product {req.ProductId} not found");
                var lineTotal = product.ProductPrice * req.Quantity;
                var item = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = product.Id,
                    Quantity = req.Quantity,
                    UnitPrice = product.ProductPrice,
                    LineTotal = lineTotal
                };
                total += lineTotal;
                _context.OrderItems.Add(item);
            }
            order.Total = total;
            await _context.SaveChangesAsync();

            order = await _context.Orders.Include(o => o.Items).ThenInclude(i => i.Product)
                .FirstAsync(o => o.Id == order.Id);
            return MapOrder(order);
        }

        public async Task<IEnumerable<OrderResponseDto>> GetOrdersAsync(int userId)
        {
            var orders = await _context.Orders.Include(o => o.Items).ThenInclude(i => i.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();
            return orders.Select(MapOrder);
        }

        public async Task<OrderResponseDto?> GetOrderAsync(int userId, int orderId)
        {
            var order = await _context.Orders.Include(o => o.Items).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
            return order == null ? null : MapOrder(order);
        }

        public async Task<bool> CancelOrderAsync(int userId, int orderId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
            if (order == null)
            {
                return false;
            }

            // Remove the order; EF will cascade-delete related OrderItems
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CompleteOrderAsync(int userId, int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
            if (order == null) return false;
            order.Status = "Completed";
            await _context.SaveChangesAsync();
            return true;
        }

        private static OrderResponseDto MapOrder(Order order)
        {
            return new OrderResponseDto
            {
                Id = order.Id,
                UserId = order.UserId,
                CreatedAt = order.CreatedAt,
                Status = order.Status,
                Total = order.Total,
                Items = order.Items.Select(i => new OrderItemResponseDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product?.ProductName ?? string.Empty,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity,
                    LineTotal = i.LineTotal
                }).ToList()
            };
        }
    }
}
