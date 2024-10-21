using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProductManagementAPI.DTOs;
using ProductManagementAPI.Models;
using ProductManagementAPI.Services;

namespace ProductManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IProductCategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IValidator<ProductCategoryDto> _validator;

        public ProductCategoriesController(IProductCategoryService categoryService, IMapper mapper, IValidator<ProductCategoryDto> validator)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductCategoryDto>>> GetAllCategories()
        {
            var categories = await _categoryService.GetAllAsync();
            var categoryDtos = _mapper.Map<IEnumerable<ProductCategoryDto>>(categories);
            return Ok(categoryDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCategoryDto>> GetCategoryById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var categoryDto = _mapper.Map<ProductCategoryDto>(category);
            return Ok(categoryDto);
        }

        [HttpPost]
        public async Task<ActionResult<ProductCategoryDto>> CreateCategory(ProductCategoryDto categoryDto)
        {
            var existingCategory = await _categoryService.GetByIdAsync(categoryDto.Id);
            if (existingCategory != null)
            {
                return BadRequest($"A category with ID {categoryDto.Id} already exists.");
            }

            var validationResult = await _validator.ValidateAsync(categoryDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var category = _mapper.Map<ProductCategory>(categoryDto);
            var createdCategory = await _categoryService.AddAsync(category);
            var createdCategoryDto = _mapper.Map<ProductCategoryDto>(createdCategory);

            return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategoryDto.Id }, createdCategoryDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, ProductCategoryDto categoryDto)
        {
            if (id != categoryDto.Id)
            {
                return BadRequest();
            }

            var validationResult = await _validator.ValidateAsync(categoryDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var category = _mapper.Map<ProductCategory>(categoryDto);
            var updatedCategory = await _categoryService.UpdateAsync(id, category);
            if (updatedCategory == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var deleted = await _categoryService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
