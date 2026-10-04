using FoodDelivery.DTOs.Product;
using FoodDelivery.Extensions;
using FoodDelivery.Service.Implements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PermissionConstants = FoodDelivery.Constants.Permissions;

    namespace FoodDelivery.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] Guid? categoryId, [FromQuery] string? q, [FromQuery] int page =1 , [FromQuery] int pageSize = 10)
            => Ok(await _productService.GetALlProductsAsync(categoryId, q, page, pageSize));
        
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
        
        [HasPermission(PermissionConstants.Products.Create)]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm]ProductCreateDto dto)
            => Ok(await _productService.CreateProductAsync(dto));

        [HasPermission(PermissionConstants.Products.Edit)]
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(Guid id, [FromForm] ProductUpdateDto dto)
            => await _productService.UpdateProductAsync(id, dto) ? Ok() : NotFound();

        [HasPermission(PermissionConstants.Products.Edit)]
        [HttpPatch("{id}/display-order")]
        public async Task<IActionResult> UpdateDisplayOrder(Guid id, [FromBody] UpdateDisplayOrderDto dto)
            => await _productService.UpdateDisplayOrderAsync(id, dto.NewDisplayOrder) ? Ok() : NotFound();

        [HasPermission(PermissionConstants.Products.ToggleAvailability)]
        [HttpPatch("{id}/toggle")]
        public async Task<IActionResult> ToggleAvailability(Guid id)
            => await _productService.ToggleProductAvailabilityAsync(id) ? Ok() : NotFound();

        [HasPermission(PermissionConstants.Products.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
            => await _productService.DeleteProductAsync(id) ? Ok() : NotFound();
    }
}