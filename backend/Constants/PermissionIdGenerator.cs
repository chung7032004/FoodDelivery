using System.Security.Cryptography;
using System.Text;

public static class PermissionIdGenerator
{
    public static Guid Generate (string input)
    {
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
        return new Guid(hash);
    }
}