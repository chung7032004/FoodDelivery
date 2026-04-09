using FoodDelivery.Common;
using FoodDelivery.DTOs.Category;
using FoodDelivery.Entities;
using FoodDelivery.Repositories.Interfaces;

namespace FoodDelivery.Service.Implementations
{

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly FoodContext _context;

        public CategoryService(
            ICategoryRepository repo,
            IUnitOfWork unitOfWork,
            FoodContext context)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<Result<IEnumerable<CategoryResponseDto>>> GetAllCategoriesAsync()
        {
            var categories = await _repo.GetAllAsync();
            if (categories == null)
            {
                return Result<IEnumerable<CategoryResponseDto>>.Failure("CATEGORY_NOT_FOUND","Không tìm thấy danh mục.");
            }
            return Result<IEnumerable<CategoryResponseDto>>.Success(categories.Select(MapToDto));
        }


        public async Task<Result<CategoryResponseDto?>> GetByIdAsync(Guid id)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null)
            {
                return Result<CategoryResponseDto?>.Failure("CATEGORY_NOT_FOUND", "Không tìm thấy danh mục.");
            }
            return Result<CategoryResponseDto?>.Success(MapToDto(category));
        }

        public async Task<Result<CategoryResponseDto>> CreateAsync(CategoryCreateDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                DisplayOrder = dto.DisplayOrder,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(category);
            await _context.SaveChangesAsync();

            return Result<CategoryResponseDto>.Success(MapToDto(category));
        }

        public async Task<Result> UpdateAsync(Guid id, CategoryUpdateDto dto)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null) return Result.Failure("CATEGORY_NOT_FOUND", "Không tìm thấy danh mục.");

            category.Name = dto.Name;
            category.Description = dto.Description;
            category.DisplayOrder = dto.DisplayOrder;
            category.UpdatedAt = DateTime.UtcNow;

            _repo.Update(category);
            await _context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null) return Result.Failure("CATEGORY_NOT_FOUND", "Không tìm thấy danh mục.");

            _repo.Delete(category);
            await _context.SaveChangesAsync();
            return Result.Success();
        }

        private static CategoryResponseDto MapToDto(Category c)
            => new()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                DisplayOrder = c.DisplayOrder
            };
    }
}