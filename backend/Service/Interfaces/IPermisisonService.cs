namespace FoodDelivery.Service.Interfaces
{
    public interface IPermissionService
    {
        Task<bool> HasPermissionAsync(Guid userId, string permission);
        Task InvalidateCacheAsync(Guid userId);
    }
}