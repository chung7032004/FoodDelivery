using FoodDelivery.DTOs.Category;
using PermissionConstants = FoodDelivery.Constants.Permissions;
using Microsoft.AspNetCore.Mvc;
using FoodDelivery.Extensions;

namespace FoodDelivery.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _service.GetAllCategoriesAsync(search, page, pageSize);
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        [HasPermission(PermissionConstants.Categories.View)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.ToActionResult();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [HasPermission(PermissionConstants.Categories.Manage)]
        public async Task<IActionResult> Create([FromForm] CategoryCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        [HasPermission(PermissionConstants.Categories.Manage)]
        public async Task<IActionResult> Update(Guid id, [FromForm] CategoryUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        [HasPermission(PermissionConstants.Categories.Manage)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            return result.ToActionResult();
        }
    }
}
