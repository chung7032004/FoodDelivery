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
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllCategoriesAsync();
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
        [HasPermission(PermissionConstants.Categories.Manage)]
        public async Task<IActionResult> Create(CategoryCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        [HasPermission(PermissionConstants.Categories.Manage)]
        public async Task<IActionResult> Update(Guid id, CategoryUpdateDto dto)
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
