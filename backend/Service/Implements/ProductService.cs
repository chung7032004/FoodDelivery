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

        // public async Task<IEnumerable<ProductResponseDto>> GetFeaturedProductsAsync()
        // {
        //     var products = await _productRepo.GetAllAsync();
        //     return products
        //         .Where(p => p.IsFeatured && p.IsAvailable)
        //         .OrderBy(p => p.DisplayOrder)
        //         .Select(MapToDto);
        // }

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
                if (request.Images != null && request.Images.Count > 0)
                {
                    foreach (var image in request.Images)
                    {
                        var uploadResult = await _fileService.SaveFileAsync(image.ImageFile, "products");
                        if (!uploadResult.IsSuccess || string.IsNullOrEmpty(uploadResult.Data))
                        {
                            // Rollback ảnh đã upload trước đó
                            foreach (var url in uploadedUrls)
                                _fileService.DeleteFile(url);

                            return HttpErrors.BadRequest<ProductResponseDto>(
                                "IMAGE_UPLOAD_FAILED",
                                "Upload ảnh thất bại cho sản phẩm với tên " + image.ImageFile.FileName);
                        }

                        uploadedUrls.Add(uploadResult.Data); // track để rollback nếu cần
                        product.ProductImages.Add(new ProductImage
                        {
                            ImageUrl = uploadResult.Data,
                            IsMain = image.IsMain,
                            DisplayOrder = image.DisplayOrder,
                            CreatedAt = DateTime.UtcNow,
                        });
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
            product.Price = dto.Price;
            // product.ImageUrl = dto.ImageUrl ?? string.Empty;
            product.IsAvailable = dto.IsAvailable;
            product.IsFeatured = dto.IsFeatured;
            product.DisplayOrder = dto.DisplayOrder;
            product.UpdatedAt = DateTime.UtcNow;

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
                RestaurantId = p.RestaurantId,  // Thêm RestaurantId
                Name = p.Name,
                Price = p.Price,
                MainImageUrl = mainImage?.ImageUrl,  // Lấy URL ảnh chính
                IsAvailable = p.IsAvailable,
                IsFeatured = p.IsFeatured,
                DisplayOrder = p.DisplayOrder,
                CreatedAt = p.CreatedAt
                // Không có UpdatedAt trong DTO mới
            };
        }
    }
}
