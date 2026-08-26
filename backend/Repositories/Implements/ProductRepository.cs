using System.Linq.Expressions;
using FoodDelivery.DTOs.Product;
using FoodDelivery.Entities;
using FoodDelivery.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly FoodContext _context;

        public ProductRepository(FoodContext context)
        {
            _context = context;
        }

        public async Task<(int,List<Product>)> GetAllAsync(Guid? categoryId = null, string? searchQuery = null, int Page = 1, int pageSize = 10)
        {
            var query = _context.Products
                .AsNoTracking()
                .Include(p=>p.Category)
                .Include(p=>p.ProductImages)
                .AsQueryable();
            if (categoryId.HasValue)
            {
                query = query.Where(p=>p.CategoryId == categoryId);
            }
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var pattern = $"%{searchQuery}%";
                query = query.Where(p=>
                EF.Functions.Like(p.Name, pattern) ||
                (p.Category != null && EF.Functions.Like(p.Category.Name, pattern)));
            }
            var totalCount =await query.CountAsync();
            var items = await query.OrderByDescending(p=>p.CreatedAt).Skip((Page-1)*pageSize).Take(pageSize).ToListAsync();
            return (totalCount,items);
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<ProductDetailResponseDto?> GetByIdDetailAsync(Guid id)
        {
             return await _context.Products
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProductDetailResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    IsAvailable = p.IsAvailable,
                    IsFeatured = p.IsFeatured,
                    DisplayOrder = p.DisplayOrder,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name,
                    RestaurantId = p.RestaurantId,
                    RestaurantName = p.Restaurant.Name,
                    Images = p.ProductImages
                        .OrderByDescending(i => i.IsMain)
                        .ThenBy(i => i.DisplayOrder)
                        .Select(i => new ProductImageDto
                        {
                            Id = i.Id,
                            ImageUrl = i.ImageUrl,
                            IsMain = i.IsMain,
                            DisplayOrder = i.DisplayOrder
                        }).ToList(),
                    ReviewCount = p.Reviews.Count(r => !r.IsHidden),
                    AverageRating = p.Reviews.Any(r => !r.IsHidden)
                        ? p.Reviews.Where(r => !r.IsHidden).Average(r => r.Rating)
                        : 0
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetByCategoryAsync(Guid categoryId)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
            return Task.CompletedTask;
        }

        public async Task<int> CountAsync(Expression<Func<Product, bool>> predicate)
        {
            return await _context.Products.CountAsync(predicate);
        }
    }
}
