using E_Benta.Data;
using E_Benta.Dtos;
using Microsoft.EntityFrameworkCore;

namespace E_Benta.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
        {
            return await _context.Products
                .Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    ProductPrice = p.ProductPrice,
                    ProductDescription = p.ProductDescription,
                    ProductCategory = p.ProductCategory,
                    UserId = p.UserId,
                    ImageUrl = p.ImageUrl
                })
                .ToListAsync();
        }

        public async Task<ProductResponseDto?> GetProductByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return null;

            return new ProductResponseDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                ProductPrice = product.ProductPrice,
                ProductDescription = product.ProductDescription,
                ProductCategory = product.ProductCategory,
                UserId = product.UserId,
                ImageUrl = product.ImageUrl
            };
        }

        public async Task<ProductResponseDto> CreateProductAsync(int userId, CreateProductDto dto)
        {
            string? imageUrl = null;
            if (dto.Image != null && dto.Image.Length > 0)
            {
                var imagesDir = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "images");
                Directory.CreateDirectory(imagesDir);
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Image.FileName)}";
                var filePath = Path.Combine(imagesDir, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(stream);
                }
                imageUrl = $"/images/{fileName}";
            }

            var product = new Models.Product
            {
                ProductName = dto.ProductName,
                ProductPrice = dto.ProductPrice,
                ProductDescription = dto.ProductDescription,
                ProductCategory = dto.ProductCategory,
                UserId = userId,
                ImageUrl = imageUrl
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new ProductResponseDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                ProductPrice = product.ProductPrice,
                ProductDescription = product.ProductDescription,
                ProductCategory = product.ProductCategory,
                UserId = product.UserId,
                ImageUrl = product.ImageUrl
            };
        }

        public async Task<bool> UpdateProductAsync(int id, UpdateProductDto dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false;
            }

            product.ProductName = dto.ProductName;
            product.ProductPrice = dto.ProductPrice;
            product.ProductDescription = dto.ProductDescription;
            product.ProductCategory = dto.ProductCategory;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
