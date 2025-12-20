using E_Benta.Data;
using E_Benta.Dtos;
using E_Benta.Models;
using Microsoft.EntityFrameworkCore;

namespace E_Benta.Services
{
    public class CartService : ICartService
    {
        private readonly AppDbContext _context;
        public CartService(AppDbContext context) { _context = context; }

        public async Task<CartResponseDto> GetCartAsync(int userId)
        {
            var cart = await _context.Carts.Include(c => c.Items).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
                cart = await _context.Carts.Include(c => c.Items).ThenInclude(i => i.Product)
                    .FirstAsync(c => c.Id == cart.Id);
            }
            return MapCart(cart);
        }

        public async Task<CartResponseDto> AddItemAsync(int userId, CreateCartItemDto dto)
        {
            var cart = await _context.Carts.Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId) ?? new Cart { UserId = userId };
            if (cart.Id == 0)
            {
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null) throw new InvalidOperationException("Product not found");

            var item = cart.Items.FirstOrDefault(i => i.ProductId == dto.ProductId);
            if (item == null)
            {
                item = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = product.Id,
                    Quantity = dto.Quantity,
                    UnitPrice = product.ProductPrice
                };
                _context.CartItems.Add(item);
            }
            else
            {
                item.Quantity += dto.Quantity;
            }

            await _context.SaveChangesAsync();
            cart = await _context.Carts.Include(c => c.Items).ThenInclude(i => i.Product)
                .FirstAsync(c => c.Id == cart.Id);
            return MapCart(cart);
        }

        public async Task<bool> UpdateItemAsync(int userId, int itemId, UpdateCartItemDto dto)
        {
            var item = await _context.CartItems.Include(i => i.Cart)
                .FirstOrDefaultAsync(i => i.Id == itemId && i.Cart.UserId == userId);
            if (item == null) return false;

            item.Quantity = dto.Quantity;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveItemAsync(int userId, int itemId)
        {
            var item = await _context.CartItems.Include(i => i.Cart)
                .FirstOrDefaultAsync(i => i.Id == itemId && i.Cart.UserId == userId);
            if (item == null) return false;
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearCartAsync(int userId)
        {
            var cart = await _context.Carts.Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null) return false;
            _context.CartItems.RemoveRange(cart.Items);
            await _context.SaveChangesAsync();
            return true;
        }

        private static CartResponseDto MapCart(Cart cart)
        {
            var items = cart.Items.Select(i => new CartItemResponseDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.Product?.ProductName ?? string.Empty,
                UnitPrice = i.UnitPrice,
                Quantity = i.Quantity
            }).ToList();
            var total = items.Sum(x => x.UnitPrice * x.Quantity);
            return new CartResponseDto
            {
                Id = cart.Id,
                UserId = cart.UserId,
                Items = items,
                Total = total
            };
        }
    }
}
