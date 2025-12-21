using E_Benta.Dtos;

namespace E_Benta.Services
{
    public interface IOrderService
    {
        Task<OrderResponseDto> BuyNowAsync(int userId, BuyNowRequestDto dto);
        Task<OrderResponseDto> CheckoutCartAsync(int userId);
        Task<OrderResponseDto> CheckoutListAsync(int userId, IEnumerable<BuyNowRequestDto> items);
        Task<IEnumerable<OrderResponseDto>> GetOrdersAsync(int userId);
        Task<OrderResponseDto?> GetOrderAsync(int userId, int orderId);
        Task<bool> CancelOrderAsync(int userId, int orderId);
        Task<bool> CompleteOrderAsync(int userId, int orderId);
    }
}
