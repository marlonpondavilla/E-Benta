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
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;
        public OrderController(IOrderService service) { _service = service; }

        [HttpPost("buy-now")]
        public async Task<ActionResult<OrderResponseDto>> BuyNow([FromBody] BuyNowRequestDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid or missing user id in token.");

            var order = await _service.BuyNowAsync(userId, dto);
            return Ok(order);
        }

        [HttpPost("checkout")]
        public async Task<ActionResult<OrderResponseDto>> CheckoutFromList([FromBody] IEnumerable<BuyNowRequestDto> items)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid or missing user id in token.");

            var order = await _service.CheckoutListAsync(userId, items);
            return Ok(order);
        }

        [HttpPost("checkout/cart")]
        public async Task<ActionResult<OrderResponseDto>> CheckoutCart()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid or missing user id in token.");

            var order = await _service.CheckoutCartAsync(userId);
            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetOrders()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid or missing user id in token.");

            var orders = await _service.GetOrdersAsync(userId);
            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<OrderResponseDto>> GetOrder(int orderId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid or missing user id in token.");

            var order = await _service.GetOrderAsync(userId, orderId);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid or missing user id in token.");

            var success = await _service.CancelOrderAsync(userId, orderId);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPost("{orderId}/complete")]
        public async Task<IActionResult> CompleteOrder(int orderId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
                return Unauthorized("Invalid or missing user id in token.");

            var success = await _service.CompleteOrderAsync(userId, orderId);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
