namespace E_Benta.Dtos
{
    public class BuyNowRequestDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class OrderItemResponseDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal { get; set; }
    }

    public class OrderResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public decimal Total { get; set; }
        public List<OrderItemResponseDto> Items { get; set; } = new();
    }
}
