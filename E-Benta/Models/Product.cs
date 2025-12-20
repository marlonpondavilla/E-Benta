namespace E_Benta.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductDescription { get; set; }
        public string ProductCategory { get; set; }

        // Image path relative to web root
        public string? ImageUrl { get; set; }

        //foreign key
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
