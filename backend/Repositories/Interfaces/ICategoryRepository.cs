using FoodDelivery.Entities;

public interface ICategoryRepository
{
    Task<(int,List<Category>)> GetAllAsync(string? search, int page, int pageSize);
    Task<Category?> GetByIdAsync(Guid id);
    Task AddAsync(Category category);
    void Update(Category category);
    void Delete(Category category);
}
