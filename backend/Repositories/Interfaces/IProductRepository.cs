using System.Linq.Expressions;
using FoodDelivery.Entities;
namespace FoodDelivery.Repositories.Interfaces;
public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);
    Task<(int,List<Product>)> GetAllAsync(Guid? categoryId = null, string? searchQuery = null, int Page = 1, int pageSize = 10);
    Task<List<Product>> GetByCategoryAsync(Guid categoryId);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);
    Task<int> CountAsync(Expression<Func<Product, bool>> predicate);
}
