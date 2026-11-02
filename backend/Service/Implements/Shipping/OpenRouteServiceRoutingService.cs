using FoodDelivery.Common;
using FoodDelivery.DTOs.Shipping;
using FoodDelivery.Service.Interfaces.IShipping;

namespace FoodDelivery.Service.Implements.Shipping;

public class OpenRouteServiceRoutingService : IRoutingService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly ILogger<OpenRouteServiceRoutingService> _logger;

    public OpenRouteServiceRoutingService(HttpClient httpClient, IConfiguration configuration, ILogger<OpenRouteServiceRoutingService> logger)
    {
        _httpClient = httpClient;
        _apiKey = configuration["Ors:ApiKey"]
            ?? configuration["OpenRouteService:ApiKey"]
            ?? throw new InvalidOperationException("Missing ORS API key. Configure Ors:ApiKey.");
        var baseUrl = configuration["Ors:BaseUrl"]
            ?? configuration["OpenRouteService:BaseUrl"]
            ?? "https://api.heigit.org/v2/directions/driving-car";
        _httpClient.BaseAddress = new Uri(baseUrl);
        _logger = logger;
    }
    public async Task<Result<RouteInfo>> GetRouteAsync(  double fromLat, double fromLon, double toLat, double toLon, CancellationToken ct = default)
    {
        try
        {
            var body = new
            {
                coordinates = new [] {new [] {fromLon, fromLat}, new [] {toLon, toLat}},
                instructions = false
            };
            using var req = new HttpRequestMessage(HttpMethod.Post, _httpClient.BaseAddress)
            {
                Content = JsonContent.Create(body)
            };
            req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
            var res = await _httpClient.SendAsync(req, ct);
            if (!res.IsSuccessStatusCode)
            {
                _logger.LogWarning("ORS failed status {Status}", res.StatusCode);
                return HttpErrors.BadRequest<RouteInfo>("ORS_FAILED","ORS request failed");
            }
            var json = await res.Content.ReadFromJsonAsync<OrsResponse>(cancellationToken: ct);
            var route = json?.Routes?.FirstOrDefault();
            if (route is null) return HttpErrors.NotFound<RouteInfo>("NO_ROUTE","No route found");

            return Result<RouteInfo>.Success(new RouteInfo(
                route.Summary.Distance / 1000.0,
                (int)Math.Ceiling(route.Summary.Duration / 60.0),
                "ORS"));
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "ORS call failed");
            return HttpErrors.InternalServerError<RouteInfo>("ORS_ERROR","ORS request failed");
        }
    }
    private class OrsResponse { public List<OrsRoute> Routes { get; set; } = new(); }
    private class OrsRoute { public OrsSummary Summary { get; set; } = new(); }
    private class OrsSummary { public double Distance { get; set; } public double Duration { get; set; } }
}