using FoodDelivery.Entities;
using Microsoft.EntityFrameworkCore;

public class CategoryRepository : ICategoryRepository
{
    private readonly FoodContext _context;

    public CategoryRepository(FoodContext context)
    {
        _context = context;
    }

    public async Task<(int, List<Category>)> GetAllAsync( string? search, int page, int pageSize)
    {
        var query = _context.Categories.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var pattern = $"%{search}%";
            query = query.Where(c => EF.Functions.Like(c.Name, pattern));
        }
        var totalCount = await query.CountAsync();
        var items = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (totalCount, items);
    }
        

    public async Task<Category?> GetByIdAsync(Guid id)
        => await _context.Categories.FindAsync(id);

    public async Task AddAsync(Category category)
        => await _context.Categories.AddAsync(category);

    public void Update(Category category)
        => _context.Categories.Update(category);

    public void Delete(Category category)
        => _context.Categories.Remove(category);
}
