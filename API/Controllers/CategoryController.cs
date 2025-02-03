using Data.Models;
using Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiEFmysql.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryRepository _categoryRepository;

        public CategoryController(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet("GetListCategories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            try
            {
                var categories = await _categoryRepository.GetCategoriesAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving categories: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryRepository.GetCategoryByIdAsync(id);

                if (category == null)
                {
                    return NotFound($"Category with ID {id} not found");
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving category: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory([FromBody] Category category)
        {
            try
            {
                var createdCategory = await _categoryRepository.CreateCategoryAsync(category);
                return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.category_id }, createdCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating category: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Category>> UpdateCategory(int id, [FromBody] Category category)
        {
            try
            {
                var existingCategory = await _categoryRepository.GetCategoryByIdAsync(id);

                if (existingCategory == null)
                {
                    return NotFound($"Category with ID {id} not found");
                }
                existingCategory.category_name = category.category_name;

                await _categoryRepository.UpdateCategoryAsync(category);

                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating category: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            try
            {
                var categoryToDelete = await _categoryRepository.GetCategoryByIdAsync(id);

                if (categoryToDelete == null)
                {
                    return NotFound($"Category with ID {id} not found");
                }

                await _categoryRepository.DeleteCategoryAsync(categoryToDelete);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting category: {ex.Message}");
            }
        }
    }
}