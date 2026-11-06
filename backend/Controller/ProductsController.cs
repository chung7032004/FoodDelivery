using FoodDelivery.DTOs.Product;
using FoodDelivery.Extensions;
using FoodDelivery.Repositories.Interfaces;
using FoodDelivery.Service.Implements;
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