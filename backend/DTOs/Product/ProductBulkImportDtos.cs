namespace FoodDelivery.DTOs.Product
{
    public class ProductBulkImportRequestDto
    {
        public Guid? RestaurantId { get; set; }
        public IFormFile? File { get; set; }
    }

    public class ProductBulkImportResponseDto
    {
        public int ImportedCount { get; set; }
        public int CreatedCategoryCount { get; set; }
    }
}
