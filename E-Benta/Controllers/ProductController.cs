using E_Benta.Dtos;
using E_Benta.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Benta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        // GET: api/product
        [HttpGet]
        public async Task<ActionResult<List<ProductResponseDto>>> GetProducts()
        {
            var products = await _service.GetAllProductsAsync();
            return Ok(products);
        }

        // GET: api/product/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDto>> GetProduct(int id)
        {
            var product = await _service.GetProductByIdAsync(id);
            if (product is null)
                return NotFound("Product with the given id was not found.");
            return Ok(product);
        }

        // POST: api/product (JSON)
        [HttpPost]
        [Consumes("application/json")]
        public async Task<ActionResult<ProductResponseDto>> CreateProductJson([FromBody] CreateProductDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Invalid or missing user id in token.");
            }

            var createdProduct = await _service.CreateProductAsync(userId, dto);
            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
        }

        // POST: api/product/with-image (multipart)
        [HttpPost("with-image")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ProductResponseDto>> CreateProductWithImage([FromForm] CreateProductDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Invalid or missing user id in token.");
            }

            var createdProduct = await _service.CreateProductAsync(userId, dto);
            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
        }

        // PUT: api/product/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto dto)
        {
            var isUpdated = await _service.UpdateProductAsync(id, dto);
            if (!isUpdated) return NotFound("Product with the given id was not found.");
            return NoContent();
        }

        // DELETE: api/product/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var isDeleted = await _service.DeleteProductAsync(id);
            if (!isDeleted) return NotFound("Product with the given id was not found.");
            return NoContent();
        }
    }
}
