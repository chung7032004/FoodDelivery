using FoodDelivery.Common;
using FoodDelivery.DTOs.Product;    
namespace FoodDelivery.Service.Implements
{
    public interface IProductService
    {
        Task<Result<PagedResponse<ProductResponseDto>>> GetALlProductsAsync(Guid? categoryId = null, string? searchQuery = null, int page = 1, int pageSize = 10);
        // Task<IEnumerable<ProductResponseDto>> GetFeaturedProductsAsync();
        Task<ProductResponseDto?> GetProductByIdAsync(Guid productId);
        Task<Result<ProductResponseDto>> CreateProductAsync(ProductCreateDto productCreateDto);
        Task<bool> UpdateProductAsync(Guid productId, ProductUpdateDto productUpdateDto);
        Task<bool> UpdateDisplayOrderAsync(Guid productId, int newDisplayOrder);
        Task<bool> ToggleProductAvailabilityAsync(Guid productId);
        Task<bool> DeleteProductAsync(Guid productId);
    }
}