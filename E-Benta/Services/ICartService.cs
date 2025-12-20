using E_Benta.Dtos;

namespace E_Benta.Services
{
    public interface ICartService
    {
        Task<CartResponseDto> GetCartAsync(int userId);
        Task<CartResponseDto> AddItemAsync(int userId, CreateCartItemDto dto);
        Task<bool> UpdateItemAsync(int userId, int itemId, UpdateCartItemDto dto);
        Task<bool> RemoveItemAsync(int userId, int itemId);
        Task<bool> ClearCartAsync(int userId);
    }
}
