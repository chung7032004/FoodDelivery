using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace FoodDelivery.Common.VnPay;

public class VnPayLibrary
{
    private readonly SortedList<string, string> _requestData = new(new VnPayCompare());
    private readonly SortedList<string, string> _responseData = new(new VnPayCompare());

    public void AddRequestData(string key, string value)
    {
        if (!string.IsNullOrEmpty(value))
            _requestData.Add(key, value);
    }

    public void AddResponseData(string key, string value)
    {
        if (!string.IsNullOrEmpty(value))
            _responseData.Add(key, value);
    }

    public string GetResponseData(string key) =>
        _responseData.TryGetValue(key, out var retValue) ? retValue : string.Empty;

    public string CreateRequestUrl(string baseUrl, string vnpHashSecret)
    {
        var data = new StringBuilder();
        foreach (var (key, value) in _requestData)
        {
            if (!string.IsNullOrEmpty(value))
            {
                data.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
            }
        }

        var queryString = data.ToString();
        var rawData = queryString.Length > 0 ? queryString.Remove(queryString.Length - 1, 1) : "";
        var vnpSecureHash = HmacSha512(vnpHashSecret, rawData);
        return $"{baseUrl}?{queryString}vnp_SecureHash={vnpSecureHash}";
    }

    public bool ValidateSignature(string inputHash, string secretKey)
    {
        if (string.IsNullOrWhiteSpace(inputHash) || inputHash.Length != 128)
            return false;

        var rawData = GetResponseDataRaw();
        var myChecksum = HmacSha512(secretKey, rawData);
        try
        {
            return CryptographicOperations.FixedTimeEquals(
                Convert.FromHexString(myChecksum),
                Convert.FromHexString(inputHash));
        }
        catch (FormatException)
        {
            return false;
        }
    }

    private string GetResponseDataRaw()
    {
        var data = new StringBuilder();
        if (_responseData.ContainsKey("vnp_SecureHashType"))
            _responseData.Remove("vnp_SecureHashType");
        if (_responseData.ContainsKey("vnp_SecureHash"))
            _responseData.Remove("vnp_SecureHash");

        foreach (var (key, value) in _responseData)
        {
            if (!string.IsNullOrEmpty(value))
                data.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
        }

        return data.Length > 0 ? data.ToString().Remove(data.Length - 1, 1) : "";
    }

    public static string HmacSha512(string key, string inputData)
    {
        var hash = new StringBuilder();
        var keyBytes = Encoding.UTF8.GetBytes(key);
        var inputBytes = Encoding.UTF8.GetBytes(inputData);
        using var hmac = new HMACSHA512(keyBytes);
        var hashValue = hmac.ComputeHash(inputBytes);
        foreach (var theByte in hashValue)
            hash.Append(theByte.ToString("x2"));
        return hash.ToString();
    }
}

public class VnPayCompare : IComparer<string>
{
    public int Compare(string? x, string? y) =>
        string.Compare(x, y, CultureInfo.InvariantCulture, CompareOptions.Ordinal);
}