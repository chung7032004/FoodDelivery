using FoodDelivery.Repositories.Interfaces;
using FoodDelivery.Service.Interfaces;
using Microsoft.Extensions.Caching.Memory;
namespace FoodDelivery.Service.Implements
{
    public class PermissionService : IPermissionService
    {
        private readonly IMemoryCache _cache;
        private readonly TimeSpan cacheDuration = TimeSpan.FromHours(1); // Thời gian cache quyền 
        private readonly IUserRepository _userRepository;
        public PermissionService(IUserRepository userRepository, IMemoryCache cache)
        {
            _userRepository = userRepository;
            _cache = cache;
        }
        public async Task<bool> HasPermissionAsync(Guid userId, string permission)
        {
            var cacheKey = $"perms:{userId}";
            if (!_cache.TryGetValue(cacheKey, out HashSet<string>? permissions))
            {
                // Nếu không có trong cache, lấy từ database
                var user = await  _userRepository.GetUserByIdWithRoleAsync(userId);
                permissions = user?.UserRoles   
                    .SelectMany(ur => ur.Role.RolePermissions)
                    .Select(rp => rp.Permission.Name)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);
                // Lưu vào cache
                _cache.Set(cacheKey, permissions, cacheDuration);
            }
            return permissions!.Contains(permission);
        }

        // Gọi khi admin thay đổi role/permission của user
        public Task InvalidateCacheAsync(Guid userId)
        {
            _cache.Remove($"perms:{userId}");
            return Task.CompletedTask;
        }
    }
}