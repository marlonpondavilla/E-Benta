using E_Benta.Dtos;

namespace E_Benta.Services
{
    public interface IProductService
    {
        Task<ProductResponseDto> CreateProductAsync(int userId, CreateProductDto dto);

        Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync();

        Task<ProductResponseDto?> GetProductByIdAsync(int id);

        Task<bool> UpdateProductAsync(int id, UpdateProductDto dto);

        Task<bool> DeleteProductAsync(int id);
    }
}
