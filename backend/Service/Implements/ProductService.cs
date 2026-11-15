using FoodDelivery.Common;
using FoodDelivery.DTOs.Product;
using FoodDelivery.Entities;
using FoodDelivery.Repositories.Interfaces;
using FoodDelivery.Service.Interfaces;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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

        public async Task<Result<PagedResponse<ProductResponseDto>>> GetALlProductsAsync(Guid? categoryId = null, string? searchQuery = null, int page = 1, int pageSize = 10, Guid? restaurantId = null)
        {
            pageSize = pageSize > 100 ? 100 : pageSize < 1 ? 10 : pageSize;
            page = page < 1 ? 1 : page;
            var (totalCount,products) = await _productRepo.GetAllAsync(categoryId,searchQuery,page,pageSize,restaurantId);
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

        public async Task<Result<decimal?>> GetProductCostPriceAsync(Guid productId)
        {
            var costPrice = await _context.Products
                .Where(product => product.Id == productId)
                .Select(product => product.CostPrice)
                .FirstOrDefaultAsync();
            if (costPrice is null && !await _context.Products.AnyAsync(product => product.Id == productId))
                return HttpErrors.NotFound<decimal?>("PRODUCT_NOT_FOUND", "Sản phẩm không tồn tại.");
            return Result<decimal?>.Success(costPrice);
        }



        public async Task<Result<ProductResponseDto>> CreateProductAsync(ProductCreateDto request)
        {
            if (request.CostPrice is < 0)
                return HttpErrors.BadRequest<ProductResponseDto>("INVALID_COST_PRICE", "Giá vốn không thể nhỏ hơn 0.");
            if (!request.CostPrice.HasValue)
                return HttpErrors.BadRequest<ProductResponseDto>("COST_PRICE_REQUIRED", "Vui lòng nhập giá vốn để theo dõi lợi nhuận.");

            if (request.RestaurantId == Guid.Empty)
                return HttpErrors.BadRequest<ProductResponseDto>("RESTAURANT_REQUIRED", "Sản phẩm phải thuộc về một nhà hàng.");

            var restaurant = await _context.Restaurants
                .FirstOrDefaultAsync(r => r.Id == request.RestaurantId
                    && r.ApplicationStatus == RestaurantApplicationStatus.Approved);
            if (restaurant == null)
                return HttpErrors.BadRequest<ProductResponseDto>("RESTAURANT_NOT_FOUND", "Nhà hàng không tồn tại hoặc chưa được duyệt.");

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == request.CategoryId
                    && (c.RestaurantId == null || c.RestaurantId == request.RestaurantId));
            if (category == null)
                return HttpErrors.BadRequest<ProductResponseDto>("CATEGORY_NOT_FOUND", "Danh mục không tồn tại hoặc không thuộc nhà hàng này.");

            int count = await _productRepo.CountAsync(p => p.RestaurantId == request.RestaurantId && p.CategoryId == request.CategoryId);
            int displayOrder = request.DisplayOrder > 0 ? request.DisplayOrder : (count + 1) * 10;

            var product = new Product
            {
                RestaurantId = request.RestaurantId,
                CategoryId = request.CategoryId,
                Name = request.Name,
                Price = request.Price,
                CostPrice = request.CostPrice,
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

        public async Task<Result<ProductBulkImportResponseDto>> ImportProductsAsync(
            Guid restaurantId,
            IFormFile file,
            CancellationToken cancellationToken)
        {
            const long maximumFileSize = 5 * 1024 * 1024;
            const int maximumProductCount = 500;

            if (restaurantId == Guid.Empty)
                return HttpErrors.BadRequest<ProductBulkImportResponseDto>("RESTAURANT_REQUIRED", "Vui lòng chọn nhà hàng.");
            if (file.Length == 0 || file.Length > maximumFileSize)
                return HttpErrors.BadRequest<ProductBulkImportResponseDto>("INVALID_IMPORT_FILE", "File Excel phải có dung lượng từ 1 byte đến 5 MB.");
            if (!string.Equals(Path.GetExtension(file.FileName), ".xlsx", StringComparison.OrdinalIgnoreCase))
                return HttpErrors.BadRequest<ProductBulkImportResponseDto>("INVALID_IMPORT_FILE_TYPE", "Chỉ hỗ trợ file Excel định dạng .xlsx.");

            var restaurantExists = await _context.Restaurants.AnyAsync(
                restaurant => restaurant.Id == restaurantId
                    && restaurant.ApplicationStatus == RestaurantApplicationStatus.Approved,
                cancellationToken);
            if (!restaurantExists)
                return HttpErrors.BadRequest<ProductBulkImportResponseDto>("RESTAURANT_NOT_FOUND", "Nhà hàng không tồn tại hoặc chưa được duyệt.");

            List<ParsedImportProduct> rows;
            try
            {
                using var stream = file.OpenReadStream();
                using var workbook = new XLWorkbook(stream);
                var worksheet = workbook.Worksheets.FirstOrDefault();
                if (worksheet == null || worksheet.FirstRowUsed() == null)
                    return HttpErrors.BadRequest<ProductBulkImportResponseDto>("EMPTY_IMPORT_FILE", "File Excel không có dữ liệu.");

                var header = worksheet.FirstRowUsed()!;
                var columns = header.CellsUsed()
                    .ToDictionary(
                        cell => cell.GetString().Trim(),
                        cell => cell.Address.ColumnNumber,
                        StringComparer.OrdinalIgnoreCase);
                var missingHeaders = new[] { "Tên món ăn", "Giá bán", "Giá vốn", "Danh mục" }
                    .Where(name => !columns.ContainsKey(name))
                    .ToList();
                if (missingHeaders.Count > 0)
                    return HttpErrors.BadRequest<ProductBulkImportResponseDto>(
                        "INVALID_IMPORT_HEADERS",
                        $"Thiếu cột bắt buộc: {string.Join(", ", missingHeaders)}.");

                var errors = new List<string>();
                var omittedErrorCount = 0;
                rows = new List<ParsedImportProduct>();
                var dataRowCount = 0;
                foreach (var row in worksheet.RowsUsed().Where(row => row.RowNumber() > header.RowNumber()))
                {
                    if (!row.CellsUsed().Any())
                        continue;

                    dataRowCount++;
                    if (dataRowCount > maximumProductCount)
                    {
                        errors.Add($"File chỉ được chứa tối đa {maximumProductCount} món ăn.");
                        break;
                    }

                    string GetValue(string columnName) =>
                        columns.TryGetValue(columnName, out var column) ? row.Cell(column).GetString().Trim() : string.Empty;

                    var name = GetValue("Tên món ăn");
                    var categoryName = GetValue("Danh mục");
                    var description = GetValue("Mô tả");
                    var rowErrors = new List<string>();

                    if (name.Length < 2)
                        rowErrors.Add("Tên món ăn phải có ít nhất 2 ký tự");
                    if (name.Length > 200)
                        rowErrors.Add("Tên món ăn không được vượt quá 200 ký tự");
                    if (string.IsNullOrWhiteSpace(categoryName))
                        rowErrors.Add("Danh mục không được để trống");

                    if (!TryReadDecimal(row.Cell(columns["Giá bán"]), out var price) || price <= 0)
                        rowErrors.Add("Giá bán phải là số lớn hơn 0");
                    if (!TryReadDecimal(row.Cell(columns["Giá vốn"]), out var costPrice) || costPrice < 0)
                        rowErrors.Add("Giá vốn phải là số lớn hơn hoặc bằng 0");

                    if (rowErrors.Count > 0)
                    {
                        if (errors.Count < 50)
                            errors.Add($"Dòng {row.RowNumber()}: {string.Join("; ", rowErrors)}.");
                        else
                            omittedErrorCount++;
                        continue;
                    }

                    rows.Add(new ParsedImportProduct(name, categoryName, description, price, costPrice));
                }

                if (errors.Count > 0)
                {
                    if (dataRowCount > maximumProductCount)
                        errors.Add($"File chỉ được chứa tối đa {maximumProductCount} món ăn.");
                    var message = string.Join(Environment.NewLine, errors);
                    if (omittedErrorCount > 0)
                        message += Environment.NewLine + $"Còn {omittedErrorCount} lỗi khác không được hiển thị.";
                    return HttpErrors.BadRequest<ProductBulkImportResponseDto>(
                        "IMPORT_VALIDATION_FAILED",
                        message);
                }
                if (rows.Count == 0)
                    return HttpErrors.BadRequest<ProductBulkImportResponseDto>("EMPTY_IMPORT_FILE", "File Excel chưa có món ăn nào để nhập.");
            }
            catch (Exception exception) when (exception is IOException
                or ArgumentException
                or InvalidDataException
                or OpenXmlPackageException)
            {
                return HttpErrors.BadRequest<ProductBulkImportResponseDto>("INVALID_EXCEL_FILE", "Không thể đọc file Excel. Vui lòng kiểm tra lại định dạng và nội dung file.");
            }

            var categories = await _context.Categories
                .Where(category => category.RestaurantId == restaurantId || category.RestaurantId == null)
                .OrderBy(category => category.RestaurantId == restaurantId ? 0 : 1)
                .ToListAsync(cancellationToken);
            var categoriesByName = categories
                .GroupBy(category => category.Name.Trim(), StringComparer.OrdinalIgnoreCase)
                .ToDictionary(group => group.Key, group => group.First(), StringComparer.OrdinalIgnoreCase);
            var createdCategoryCount = 0;
            var importedProducts = new List<Product>(rows.Count);

            foreach (var row in rows)
            {
                if (!categoriesByName.TryGetValue(row.CategoryName, out var category))
                {
                    category = new Category
                    {
                        Name = row.CategoryName,
                        RestaurantId = restaurantId,
                        ImageUrl = string.Empty,
                        CreatedAt = DateTime.UtcNow
                    };
                    categoriesByName.Add(row.CategoryName, category);
                    _context.Categories.Add(category);
                    createdCategoryCount++;
                }

                importedProducts.Add(new Product
                {
                    Id = Guid.NewGuid(),
                    RestaurantId = restaurantId,
                    Category = category,
                    Name = row.Name,
                    Description = row.Description,
                    Price = row.Price,
                    CostPrice = row.CostPrice,
                    IsAvailable = true,
                    DisplayOrder = 0,
                    CreatedAt = DateTime.UtcNow,
                    ProductImages = new List<ProductImage>()
                });
            }

            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var categoryCounts = await _context.Products
                    .Where(product => product.RestaurantId == restaurantId)
                    .GroupBy(product => product.CategoryId)
                    .Select(group => new { CategoryId = group.Key, Count = group.Count() })
                    .ToDictionaryAsync(item => item.CategoryId, item => item.Count, cancellationToken);

                foreach (var product in importedProducts)
                {
                    var count = categoryCounts.GetValueOrDefault(product.Category.Id);
                    product.DisplayOrder = (count + 1) * 10;
                    categoryCounts[product.Category.Id] = count + 1;
                }

                _context.Products.AddRange(importedProducts);
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }

            return Result<ProductBulkImportResponseDto>.Success(new ProductBulkImportResponseDto
            {
                ImportedCount = importedProducts.Count,
                CreatedCategoryCount = createdCategoryCount
            });
        }

        private static bool TryReadDecimal(IXLCell cell, out decimal value)
        {
            var text = cell.GetString().Trim();
            return decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out value)
                || decimal.TryParse(text, NumberStyles.Number, CultureInfo.GetCultureInfo("vi-VN"), out value);
        }

        private sealed record ParsedImportProduct(
            string Name,
            string CategoryName,
            string Description,
            decimal Price,
            decimal CostPrice);

        public async Task<bool> UpdateProductAsync(Guid productId, ProductUpdateDto dto)
        {
            if (dto.CostPrice is null or < 0) return false;
            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null) return false;

            if (dto.CategoryId.HasValue)
            {
                var categoryExists = await _context.Categories.AnyAsync(category =>
                    category.Id == dto.CategoryId.Value
                    && (category.RestaurantId == null || category.RestaurantId == product.RestaurantId));
                if (!categoryExists) return false;
                product.CategoryId = dto.CategoryId.Value;
            }

            product.Name = dto.Name;
            product.Description = dto.Description ?? string.Empty;
            product.Price = dto.Price;
            product.CostPrice = dto.CostPrice;
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
