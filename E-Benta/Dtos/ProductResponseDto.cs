namespace E_Benta.Dtos
{
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductDescription { get; set; }
        public string ProductCategory { get; set; }
        public int UserId { get; set; } 
        public string? ImageUrl { get; set; }
    }
}
