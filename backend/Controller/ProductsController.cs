using FoodDelivery.DTOs.Product;
using FoodDelivery.Extensions;
using FoodDelivery.Repositories.Interfaces;
using FoodDelivery.Service.Implements;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using PermissionConstants = FoodDelivery.Constants.Permissions;

    namespace FoodDelivery.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IStaffRepository _staffRepository;

        public ProductsController(
            IProductService productService,
            IRestaurantRepository restaurantRepository,
            IStaffRepository staffRepository)
        {
            _productService = productService;
            _restaurantRepository = restaurantRepository;
            _staffRepository = staffRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] Guid? categoryId, [FromQuery] string? q, [FromQuery] int page =1 , [FromQuery] int pageSize = 10, [FromQuery] Guid? restaurantId = null)
            => Ok(await _productService.GetALlProductsAsync(categoryId, q, page, pageSize, restaurantId));
        
        // [HasPermission(PermissionConstants.Products.View)]
        // [HttpGet("featured")]
        // public async Task<IActionResult> GetFeaturedProducts()
        //     => Ok(await _productService.GetFeaturedProductsAsync());
        
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductById(Guid productId)
        {
            var result = await _productService.GetProductDetailByIdAsync(productId);
            return result.ToActionResult();
        }

        [HasPermission(PermissionConstants.Products.ViewCostPrice)]
        [HttpGet("{id}/cost-price")]
        public async Task<IActionResult> GetCostPrice(Guid id)
        {
            return (await _productService.GetProductCostPriceAsync(id)).ToActionResult();
        }
        
        [HasPermission(PermissionConstants.Products.Create)]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm]ProductCreateDto dto)
        {
            if (!User.IsInRole("Admin"))
            {
                var restaurantId = await GetManagedRestaurantIdAsync();
                if (!restaurantId.HasValue)
                    return Forbid();
                dto.RestaurantId = restaurantId.Value;
            }

            var result = await _productService.CreateProductAsync(dto);
            return result.ToActionResult();
        }

        [HasPermission(PermissionConstants.Products.Create)]
        [HttpGet("import-template")]
        [Produces("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
        public IActionResult DownloadImportTemplate(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using var workbook = new XLWorkbook();
            var menuSheet = workbook.Worksheets.Add("ThucDon");
            menuSheet.Cell(1, 1).Value = "Tên món ăn";
            menuSheet.Cell(1, 2).Value = "Giá bán";
            menuSheet.Cell(1, 3).Value = "Giá vốn";
            menuSheet.Cell(1, 4).Value = "Mô tả";
            menuSheet.Cell(1, 5).Value = "Danh mục";
            menuSheet.Row(1).Style.Font.Bold = true;
            menuSheet.Columns(1, 5).AdjustToContents();
            menuSheet.SheetView.FreezeRows(1);

            var instructions = workbook.Worksheets.Add("HuongDan");
            instructions.Cell(1, 1).Value = "Hướng dẫn nhập thực đơn";
            instructions.Cell(2, 1).Value = "Điền dữ liệu từ dòng 2 của sheet ThucDon.";
            instructions.Cell(3, 1).Value = "Tên món ăn, Giá bán, Giá vốn và Danh mục là các cột bắt buộc.";
            instructions.Cell(4, 1).Value = "Giá bán phải lớn hơn 0; Giá vốn phải lớn hơn hoặc bằng 0.";
            instructions.Cell(5, 1).Value = "Danh mục chưa có sẽ được tự động tạo cho nhà hàng.";
            instructions.Cell(6, 1).Value = "Tối đa 500 món mỗi lần nhập. Ảnh món ăn có thể tải lên sau khi nhập.";
            instructions.Column(1).Width = 100;

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "food-delivery-products-template.xlsx");
        }

        [HasPermission(PermissionConstants.Products.Create)]
        [HttpPost("bulk-import")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(6 * 1024 * 1024)]
        [RequestFormLimits(MultipartBodyLengthLimit = 6 * 1024 * 1024)]
        [ProducesResponseType(typeof(FoodDelivery.Common.Result<ProductBulkImportResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FoodDelivery.Common.Result<ProductBulkImportResponseDto>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> BulkImport(
            [FromForm] ProductBulkImportRequestDto request,
            CancellationToken cancellationToken)
        {
            if (request.File == null)
                return BadRequest(FoodDelivery.Common.Result.Failure("IMPORT_FILE_REQUIRED", "Vui lòng chọn file Excel."));

            Guid restaurantId;
            if (User.IsInRole("Admin"))
            {
                if (!request.RestaurantId.HasValue || request.RestaurantId == Guid.Empty)
                    return BadRequest(FoodDelivery.Common.Result.Failure("RESTAURANT_REQUIRED", "Vui lòng nhập mã nhà hàng cần nhập thực đơn."));
                restaurantId = request.RestaurantId.Value;
            }
            else
            {
                var managedRestaurantId = await GetManagedRestaurantIdAsync();
                if (!managedRestaurantId.HasValue)
                    return Forbid();
                restaurantId = managedRestaurantId.Value;
            }

            var result = await _productService.ImportProductsAsync(restaurantId, request.File, cancellationToken);
            return result.ToActionResult();
        }

        [HasPermission(PermissionConstants.Products.UpdateCostPrice)]
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(Guid id, [FromForm] ProductUpdateDto dto)
        {
            return await _productService.UpdateProductAsync(id, dto) ? Ok() : NotFound();
        }

        [HasPermission(PermissionConstants.Products.Edit)]
        [HttpPatch("{id}/display-order")]
        public async Task<IActionResult> UpdateDisplayOrder(Guid id, [FromBody] UpdateDisplayOrderDto dto)
        {
            return await _productService.UpdateDisplayOrderAsync(id, dto.NewDisplayOrder) ? Ok() : NotFound();
        }

        [HasPermission(PermissionConstants.Products.ToggleAvailability)]
        [HttpPatch("{id}/toggle")]
        public async Task<IActionResult> ToggleAvailability(Guid id)
        {
            return await _productService.ToggleProductAvailabilityAsync(id) ? Ok() : NotFound();
        }

        [HasPermission(PermissionConstants.Products.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return await _productService.DeleteProductAsync(id) ? Ok() : NotFound();
        }

        private async Task<Guid?> GetManagedRestaurantIdAsync()
        {
            var restaurant = await _restaurantRepository.GetRestaurantByUserIdAsync(User.GetUserId());
            if (restaurant?.ApplicationStatus != RestaurantApplicationStatus.Approved)
                return null;

            if (User.IsInRole("RestaurantOwner"))
                return restaurant.Id;

            if (User.IsInRole("RestaurantStaff")
                && await _staffRepository.IsStaffOfRestaurantAsync(User.GetUserId(), restaurant.Id))
                return restaurant.Id;

            return null;
        }
    }
}