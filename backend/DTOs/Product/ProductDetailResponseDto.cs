namespace FoodDelivery.DTOs.Product
{
    public class ProductDetailResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsFeatured { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Ảnh — trả full list, FE tự chọn ảnh chính (IsMain) làm hero, còn lại làm gallery
        public List<ProductImageDto> Images { get; set; } = new();

        // Category context
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        // Restaurant context
        public Guid RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;

        // Rating tổng hợp — tính sẵn ở query, KHÔNG trả nguyên list Reviews vào đây
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
    }

    public class ProductImageDto
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsMain { get; set; }
        public int DisplayOrder { get; set; }
    }
}