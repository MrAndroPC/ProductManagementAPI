using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProductManagementAPI.DTOs;
using ProductManagementAPI.Models;
using ProductManagementAPI.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace ProductManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly IValidator<ProductDto> _validator;
        private readonly IProductCategoryService _categoryService;

        public ProductsController(IProductService productService, IProductCategoryService categoryService, IMapper mapper, IValidator<ProductDto> validator)
        {
            _productService = productService;
            _mapper = mapper;
            _validator = validator;
            _categoryService = categoryService;
        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns>A list of products</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Получить все продукты", Description = "Возвращает список всех продуктов.")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            var products = await _productService.GetAllAsync();
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(productDtos);
        }

        /// <summary>
        /// Gets a product by ID.
        /// </summary>
        /// <param name="id">The product ID</param>
        /// <returns>A product</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Получение продукта по ID", Description = "Возвращает один продукт по его ID.")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var productDto = _mapper.Map<ProductDto>(product);
            return Ok(productDto);
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="productDto">The product data</param>
        /// <returns>The created product</returns>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Создание нового продукта", Description = "Создает новый продукт с указанными данными.")]
        public async Task<ActionResult<ProductDto>> CreateProduct(ProductDto productDto)
        {
            var existingProduct = await _productService.GetByIdAsync(productDto.Id);
            if (existingProduct != null)
            {
                return BadRequest($"Товар с ID {productDto.Id} уже существует.");
            }

            var category = await _categoryService.GetByIdAsync(productDto.CategoryId);
            if (category == null)
            {
                return BadRequest($"Категория с ID {productDto.CategoryId} не существует.");
            }

            var validationResult = await _validator.ValidateAsync(productDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var product = _mapper.Map<Product>(productDto);
            var createdProduct = await _productService.AddAsync(product);
            var createdProductDto = _mapper.Map<ProductDto>(createdProduct);

            return CreatedAtAction(nameof(GetProductById), new { id = createdProductDto.Id }, createdProductDto);
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">The product ID</param>
        /// <param name="productDto">The updated product data</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Обновление существующего продукта", Description = "Обновление сведений о существующем продукте.")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDto productDto)
        {
            var category = await _categoryService.GetByIdAsync(productDto.CategoryId);
            if (category == null)
            {
                return BadRequest($"Категория с ID {productDto.CategoryId} не существует.");
            }

            if (id != productDto.Id)
            {
                return BadRequest();
            }

            var validationResult = await _validator.ValidateAsync(productDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var product = _mapper.Map<Product>(productDto);
            var updatedProduct = await _productService.UpdateAsync(id, product);
            if (updatedProduct == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a product by ID.
        /// </summary>
        /// <param name="id">The product ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Удалить продукт", Description = "Удаление продукта по его ID.")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deleted = await _productService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
