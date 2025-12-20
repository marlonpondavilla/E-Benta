using Microsoft.AspNetCore.Http;

namespace E_Benta.Dtos
{
    public class CreateProductWithImageDto
    {
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductDescription { get; set; }
        public string ProductCategory { get; set; }
        public IFormFile? Image { get; set; }
    }
}
