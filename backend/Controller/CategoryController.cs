using FoodDelivery.DTOs.Category;
using PermissionConstants = FoodDelivery.Constants.Permissions;
using Microsoft.AspNetCore.Mvc;

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

        [HasPermission(PermissionConstants.Categories.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllCategoriesAsync();
            if(!result.IsSuccess)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        [HasPermission(PermissionConstants.Categories.View)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [HasPermission(PermissionConstants.Categories.Manage)]
        public async Task<IActionResult> Create(CategoryCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            if (!result.IsSuccess) return BadRequest(result);
            return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
        }

        [HttpPut("{id}")]
        [HasPermission(PermissionConstants.Categories.Manage)]
        public async Task<IActionResult> Update(Guid id, CategoryUpdateDto dto)
        {
            var success = await _service.UpdateAsync(id, dto);
            if (!success.IsSuccess) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [HasPermission(PermissionConstants.Categories.Manage)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success.IsSuccess) return NotFound();
            return NoContent();
        }
    }
}
