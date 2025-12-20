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
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;
        public CartController(ICartService service) { _service = service; }

        [HttpGet]
        public async Task<ActionResult<CartResponseDto>> GetCart()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid or missing user id in token.");

            var cart = await _service.GetCartAsync(userId);
            return Ok(cart);
        }

        [HttpPost("items")]
        public async Task<ActionResult<CartResponseDto>> AddItem([FromBody] CreateCartItemDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid or missing user id in token.");

            var cart = await _service.AddItemAsync(userId, dto);
            return Ok(cart);
        }

        [HttpPut("items/{itemId}")]
        public async Task<IActionResult> UpdateItem(int itemId, [FromBody] UpdateCartItemDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid or missing user id in token.");

            var success = await _service.UpdateItemAsync(userId, itemId, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("items/{itemId}")]
        public async Task<IActionResult> RemoveItem(int itemId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid or missing user id in token.");

            var success = await _service.RemoveItemAsync(userId, itemId);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid or missing user id in token.");

            var success = await _service.ClearCartAsync(userId);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
