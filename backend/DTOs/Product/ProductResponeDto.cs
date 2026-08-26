namespace FoodDelivery.DTOs.Product
{
    public class ProductResponseDto
    {
        public Guid Id { get; set; }                    // ID vừa tạo
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? MainImageUrl { get; set; }       // Ảnh chính (quan trọng nhất)
        public bool IsAvailable { get; set; }
        public bool IsFeatured { get; set; }
        public int DisplayOrder { get; set; }           // Thứ tự đã được tính toán
        public DateTime CreatedAt { get; set; }         // Thời gian tạo
        
        // Thông tin cho context
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public Guid RestaurantId { get; set; }
    }
}