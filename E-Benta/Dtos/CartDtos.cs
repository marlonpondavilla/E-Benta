namespace E_Benta.Dtos
{
    public class CartItemResponseDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }

    public class CartResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<CartItemResponseDto> Items { get; set; } = new();
        public decimal Total { get; set; }
    }

    public class CreateCartItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateCartItemDto
    {
        public int Quantity { get; set; }
    }
}
