using FoodDelivery.Common;
using FoodDelivery.DTOs.Category;
using FoodDelivery.Entities;
using FoodDelivery.Service.Interfaces;


namespace FoodDelivery.Service.Implementations
{

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;
        private readonly FoodContext _context;
        private readonly IFileService _fileService;

        public CategoryService(
            ICategoryRepository repo,
            FoodContext context,
            IFileService fileService)
        {
            _repo = repo;
            _context = context;
            _fileService = fileService;
        }

        public async Task<Result<PagedResponse<CategoryResponseDto>>> GetAllCategoriesAsync(string? search, int page, int pageSize)
        {
            var (totalCount, categories) = await _repo.GetAllAsync(search, page, pageSize);
            if (categories == null)
            {
                return HttpErrors.NotFound<PagedResponse<CategoryResponseDto>>("CATEGORY_NOT_FOUND", "Không tìm thấy danh mục.");
            }
            var responseMeta = new PaginationMeta(
                Page : page,
                PageSize : pageSize,
                TotalCount : totalCount,
                TotalPages : (int)Math.Ceiling((double)totalCount/pageSize)
            );
            var data = categories.Select(MapToDto).ToList();
            var pagedResponse = new PagedResponse<CategoryResponseDto>
            (
                Data : data,
                Meta : responseMeta
            );
            return Result<PagedResponse<CategoryResponseDto>>.Success(pagedResponse);
        }

        public async Task<Result<CategoryResponseDto?>> GetByIdAsync(Guid id)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null)
            {
                return HttpErrors.NotFound<CategoryResponseDto?>("CATEGORY_NOT_FOUND", "Không tìm thấy danh mục.");
            }
            return Result<CategoryResponseDto?>.Success(MapToDto(category));
        }

        public async Task<Result<CategoryResponseDto>> CreateAsync(CategoryCreateDto dto)
        {
            var imageUrl = dto.ImageUrl;
            if (dto.ImageFile != null)
            {
                var uploadResult = await _fileService.SaveFileAsync(dto.ImageFile, "category");
                if (!uploadResult.IsSuccess || string.IsNullOrEmpty(uploadResult.Data))
                {
                    return Result<CategoryResponseDto>.Failure(uploadResult.ErrorCode ?? "FILE_UPLOAD_FAILED", uploadResult.Message ?? "Tải ảnh danh mục thất bại");
                }
                imageUrl = uploadResult.Data;
            }

            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                DisplayOrder = dto.DisplayOrder,
                ImageUrl = imageUrl ?? string.Empty,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(category);
            await _context.SaveChangesAsync();

            return Result<CategoryResponseDto>.Success(MapToDto(category));
        }

        public async Task<Result> UpdateAsync(Guid id, CategoryUpdateDto dto)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null) return HttpErrors.NotFound("CATEGORY_NOT_FOUND", "Không tìm thấy danh mục.");

            category.Name = dto.Name;
            category.Description = dto.Description;
            category.DisplayOrder = dto.DisplayOrder;

            if (dto.ImageFile != null)
            {
                if (!string.IsNullOrWhiteSpace(category.ImageUrl))
                {
                    _fileService.DeleteFile(category.ImageUrl);
                }

                var uploadResult = await _fileService.SaveFileAsync(dto.ImageFile, "category");
                if (!uploadResult.IsSuccess || string.IsNullOrEmpty(uploadResult.Data))
                {
                    return Result.Failure(uploadResult.ErrorCode ?? "FILE_UPLOAD_FAILED", uploadResult.Message ?? "Tải ảnh danh mục thất bại");
                }

                category.ImageUrl = uploadResult.Data;
            }
            else if (!string.IsNullOrWhiteSpace(dto.ImageUrl))
            {
                category.ImageUrl = dto.ImageUrl;
            }

            category.UpdatedAt = DateTime.UtcNow;

            _repo.Update(category);
            await _context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null) return HttpErrors.NotFound("CATEGORY_NOT_FOUND", "Không tìm thấy danh mục.");

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
                DisplayOrder = c.DisplayOrder,
                ImageUrl = c.ImageUrl,
                CreatedAt = c.CreatedAt
            };
    }
}