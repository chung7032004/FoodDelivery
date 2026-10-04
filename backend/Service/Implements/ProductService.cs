using FoodDelivery.Common;
using FoodDelivery.DTOs.Product;
using FoodDelivery.Entities;
using FoodDelivery.Repositories.Interfaces;
using FoodDelivery.Service.Interfaces;

namespace FoodDelivery.Service.Implements
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly FoodContext _context;
        private readonly IFileService _fileService;
        public ProductService(
            IProductRepository productRepo,
            FoodContext context,
            IFileService fileService)
        {
            _productRepo = productRepo;
            _context = context;
            _fileService = fileService;
            _context = context;
        }

        public async Task<Result<PagedResponse<ProductResponseDto>>> GetALlProductsAsync(Guid? categoryId = null, string? searchQuery = null, int page = 1, int pageSize = 10)
        {
            pageSize = pageSize > 100 ? 100 : pageSize < 1 ? 10 : pageSize;
            page = page < 1 ? 1 : page;
            var (totalCount,products) = await _productRepo.GetAllAsync(categoryId,searchQuery,page,pageSize);
            var productsResponse = products.Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description ?? string.Empty,
                Price = p.Price,
                MainImageUrl = p.ProductImages.Where(i=>i.IsMain).Select(i=>i.ImageUrl).FirstOrDefault()
                            ?? p.ProductImages.Select(i=>i.ImageUrl).FirstOrDefault(),
                IsAvailable = p.IsAvailable,
                IsFeatured = p.IsFeatured,
                DisplayOrder = p.DisplayOrder,
                CreatedAt = p.CreatedAt,
                CategoryId = p.Category.Id,
                CategoryName = p.Category.Name,
                RestaurantId = p.RestaurantId
            }).ToList();
            var responseMeta = new PaginationMeta(
                Page : page,
                PageSize : productsResponse.Count,
                TotalCount : totalCount,
                TotalPages : (int)Math.Ceiling((double)totalCount/pageSize)
            );
            var response = new PagedResponse<ProductResponseDto>(
                Meta : responseMeta,
                Data : productsResponse
            );
            return Result<PagedResponse<ProductResponseDto>>.Success(response);
        }

        public async Task<Result<ProductDetailResponseDto?>> GetProductDetailByIdAsync(Guid productId)
        {
            var product = await _productRepo.GetByIdDetailAsync(productId);
            return product == null ? HttpErrors.NotFound<ProductDetailResponseDto?>("PRODUCT_NOT_FOUND", "Sản phẩm không tồn tại") : Result<ProductDetailResponseDto?>.Success(product);
        }



        public async Task<Result<ProductResponseDto>> CreateProductAsync(ProductCreateDto request)
        {
            int count = await _productRepo.CountAsync(p => p.CategoryId == request.CategoryId);
            int displayOrder = request.DisplayOrder > 0 ? request.DisplayOrder : (count + 1) * 10;

            var product = new Product
            {
                RestaurantId = request.RestaurantId,
                CategoryId = request.CategoryId,
                Name = request.Name,
                Price = request.Price,
                Description = request.Description ?? string.Empty,
                IsAvailable = request.IsAvailable,
                IsFeatured = request.IsFeatured,
                DisplayOrder = displayOrder,
                CreatedAt = DateTime.UtcNow,
                ProductImages = new List<ProductImage>()
            };

            var uploadedUrls = new List<string>();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var files = request.ImageFiles ?? new List<IFormFile>();
                if (files.Count > 0)
                {
                    var mainIndex = request.MainImageIndex ?? 0;
                    mainIndex = Math.Clamp(mainIndex, 0, files.Count - 1);

                    for (var i = 0; i < files.Count; i++)
                    {
                        var imageFile = files[i];
                        var uploadResult = await _fileService.SaveFileAsync(imageFile, "products");
                        if (!uploadResult.IsSuccess || string.IsNullOrEmpty(uploadResult.Data))
                        {
                            foreach (var url in uploadedUrls)
                                _fileService.DeleteFile(url);

                            return HttpErrors.BadRequest<ProductResponseDto>(
                                "IMAGE_UPLOAD_FAILED",
                                "Upload ảnh thất bại cho sản phẩm với tên " + imageFile.FileName);
                        }

                        uploadedUrls.Add(uploadResult.Data);
                        product.ProductImages.Add(new ProductImage
                        {
                            ImageUrl = uploadResult.Data,
                            IsMain = i == mainIndex,
                            DisplayOrder = i,
                            CreatedAt = DateTime.UtcNow,
                        });
                    }

                    if (!product.ProductImages.Any(img => img.IsMain))
                    {
                        product.ProductImages.First().IsMain = true;
                    }
                }

                await _productRepo.AddAsync(product);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Result<ProductResponseDto>.Success(MapToDto(product));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                foreach (var url in uploadedUrls)
                    _fileService.DeleteFile(url);

                return HttpErrors.BadRequest<ProductResponseDto>(
                    "CREATE_PRODUCT_FAILED",
                    "Tạo sản phẩm thất bại: " + ex.Message);
            }
        }

        public async Task<bool> UpdateProductAsync(Guid productId, ProductUpdateDto dto)
        {
            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null) return false;

            product.Name = dto.Name;
            product.Description = dto.Description ?? string.Empty;
            product.Price = dto.Price;
            product.IsAvailable = dto.IsAvailable;
            product.IsFeatured = dto.IsFeatured;
            product.DisplayOrder = dto.DisplayOrder;
            product.UpdatedAt = DateTime.UtcNow;

            var imageFiles = dto.ImageFiles ?? new List<IFormFile>();
            if (imageFiles.Count > 0)
            {
                product.ProductImages ??= new List<ProductImage>();

                if (dto.MainImageIndex.HasValue)
                {
                    foreach (var img in product.ProductImages)
                    {
                        img.IsMain = false;
                    }
                }

                var mainIndex = dto.MainImageIndex ?? 0;
                if (dto.MainImageIndex.HasValue)
                {
                    mainIndex = Math.Clamp(mainIndex, 0, imageFiles.Count - 1);
                }

                for (var i = 0; i < imageFiles.Count; i++)
                {
                    var imageFile = imageFiles[i];
                    var uploadResult = await _fileService.SaveFileAsync(imageFile, "products");
                    if (!uploadResult.IsSuccess || string.IsNullOrEmpty(uploadResult.Data))
                    {
                        return false;
                    }

                    var isMain = dto.MainImageIndex.HasValue
                        ? i == mainIndex
                        : !product.ProductImages.Any(img => img.IsMain) && i == 0;

                    product.ProductImages.Add(new ProductImage
                    {
                        ImageUrl = uploadResult.Data,
                        IsMain = isMain,
                        DisplayOrder = product.ProductImages.Count,
                        CreatedAt = DateTime.UtcNow,
                    });
                }

                if (!product.ProductImages.Any(img => img.IsMain))
                {
                    product.ProductImages.First().IsMain = true;
                }
            }

            await _productRepo.UpdateAsync(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateDisplayOrderAsync(Guid productId, int newDisplayOrder)
        {
            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null) return false;

            product.DisplayOrder = newDisplayOrder;
            product.UpdatedAt = DateTime.UtcNow;

            await _productRepo.UpdateAsync(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ToggleProductAvailabilityAsync(Guid productId)
        {
            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null) return false;

            product.IsAvailable = !product.IsAvailable;
            product.UpdatedAt = DateTime.UtcNow;

            await _productRepo.UpdateAsync(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProductAsync(Guid productId)
        {
            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null) return false;

            await _productRepo.DeleteAsync(product);
            await _context.SaveChangesAsync();
            return true;
        }

       private static ProductResponseDto MapToDto(Product p)
        {
            // Lấy ảnh chính (IsMain = true) hoặc ảnh đầu tiên nếu không có ảnh chính
            var mainImage = p.ProductImages?
                .FirstOrDefault(img => img.IsMain) ?? p.ProductImages?.FirstOrDefault();
            
            return new ProductResponseDto
            {
                Id = p.Id,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name ?? string.Empty,
                RestaurantId = p.RestaurantId,
                Name = p.Name,
                Description = p.Description ?? string.Empty,
                Price = p.Price,
                MainImageUrl = mainImage?.ImageUrl,
                IsAvailable = p.IsAvailable,
                IsFeatured = p.IsFeatured,
                DisplayOrder = p.DisplayOrder,
                CreatedAt = p.CreatedAt
            };
        }
    }
}
