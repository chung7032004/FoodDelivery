using FoodDelivery.Common;
using FoodDelivery.DTOs.Category;

public interface ICategoryService
{
    // Task<Result<IEnumerable<CategoryResponseDto>>> GetAllCategoriesAsync();
    Task<Result<PagedResponse<CategoryResponseDto>>> GetAllCategoriesAsync(string? search,int page, int pageSize);
    Task<Result<CategoryResponseDto?>> GetByIdAsync(Guid id);
    Task<Result<CategoryResponseDto>> CreateAsync(CategoryCreateDto dto);
    Task<Result> UpdateAsync(Guid id, CategoryUpdateDto dto);
    Task<Result> DeleteAsync(Guid id);
}


