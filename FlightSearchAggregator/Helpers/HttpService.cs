namespace FlightSearchAggregator.Helpers;

public class HttpService
{

    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<HttpService> _logger;

    public HttpService(IHttpClientFactory client, ILogger<HttpService> logger)
    {
        _clientFactory = client;
        _logger = logger;
    }

    public async Task<T?> ExecuteGetRequest<T>( string clientName, string uri)
    {
        try
        {
            var client = _clientFactory.CreateClient(clientName);
            var response = await client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"GET request to {uri} failed.");
            throw;
        }
    }

    public async Task<TDestination?> ExecutePostRequest<TSource, TDestination>(string clientName, string uri, TSource content)
    {
        try
        {
            var client = _clientFactory.CreateClient(clientName);
            var response = await client.PostAsJsonAsync(uri, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TDestination>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"POST request to {uri} with content {typeof(TSource).Name} failed.");
            throw;
        }
    }
}