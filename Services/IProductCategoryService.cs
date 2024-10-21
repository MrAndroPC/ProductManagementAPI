using ProductManagementAPI.Models;

namespace ProductManagementAPI.Services
{
    public interface IProductCategoryService
    {
        Task<IEnumerable<ProductCategory>> GetAllAsync();
        Task<ProductCategory> GetByIdAsync(int id);
        Task<ProductCategory> AddAsync(ProductCategory category);
        Task<ProductCategory> UpdateAsync(int id, ProductCategory category);
        Task<bool> DeleteAsync(int id);
    }
}
