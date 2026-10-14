using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using FoodDelivery.Constants;
using FoodDelivery.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Data;

public sealed class InitialAdminSeeder
{
    private readonly FoodContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<InitialAdminSeeder> _logger;

    public InitialAdminSeeder(
        FoodContext context,
        IConfiguration configuration,
        ILogger<InitialAdminSeeder> logger)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var section = _configuration.GetSection("InitialAdmin");
        var email = section["Email"]?.Trim();
        var password = section["Password"];
        var fullName = section["FullName"]?.Trim();
        var phone = section["Phone"]?.Trim();

        if (email is null && password is null && fullName is null && phone is null)
        {
            _logger.LogInformation(
                "Initial admin seeding skipped because InitialAdmin configuration is not set.");
            return;
        }

        if (string.IsNullOrWhiteSpace(email)
            || string.IsNullOrWhiteSpace(password)
            || string.IsNullOrWhiteSpace(fullName)
            || string.IsNullOrWhiteSpace(phone))
        {
            throw new InvalidOperationException(
                "InitialAdmin:Email, Password, FullName, and Phone must all be configured.");
        }

        if (password.Length < 12)
            throw new InvalidOperationException("InitialAdmin:Password must be at least 12 characters.");

        if (!string.IsNullOrWhiteSpace(new string(email.Where(char.IsWhiteSpace).ToArray()))
            || !MailAddress.TryCreate(email, out var mailAddress)
            || !string.Equals(mailAddress.Address, email, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("InitialAdmin:Email must be a valid email address.");
        }

        if (!System.Text.RegularExpressions.Regex.IsMatch(phone, @"^0\d{9}$"))
            throw new InvalidOperationException("InitialAdmin:Phone must contain 10 digits and start with 0.");

        var adminRole = await _context.Roles
            .SingleOrDefaultAsync(role => role.Id == RoleIds.Admin, cancellationToken);
        if (adminRole is null)
            throw new InvalidOperationException("The Admin role is missing. Apply database migrations before seeding.");

        var normalizedEmail = email.ToLowerInvariant();
        var user = await _context.Users
            .Include(account => account.UserRoles)
            .SingleOrDefaultAsync(
                account => account.Email.ToLower() == normalizedEmail,
                cancellationToken);

        if (user is null)
        {
            using var hmac = new HMACSHA512();
            user = new User
            {
                Email = normalizedEmail,
                FullName = fullName,
                Phone = phone,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                PasswordSalt = hmac.Key,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
            _context.Users.Add(user);
            _context.UserRoles.Add(new UserRole
            {
                User = user,
                RoleId = adminRole.Id
            });

            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Created initial admin account {Email}.", normalizedEmail);
            return;
        }

        if (!user.UserRoles.Any(userRole => userRole.RoleId == adminRole.Id))
        {
            _context.UserRoles.Add(new UserRole
            {
                UserId = user.Id,
                RoleId = adminRole.Id
            });
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Granted the Admin role to configured account {Email}.", normalizedEmail);
            return;
        }

        _logger.LogInformation("Initial admin account {Email} already exists.", normalizedEmail);
    }
}
