namespace FoodDelivery.DTOs.Product
{
    public class ProductUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsFeatured { get; set; }
        public int DisplayOrder { get; set; }
        public List<IFormFile>? ImageFiles { get; set; }
        public int? MainImageIndex { get; set; }
    }
}