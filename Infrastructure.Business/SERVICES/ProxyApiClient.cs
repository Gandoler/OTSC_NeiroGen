using System.Net.Http.Json;
using Domain.Services.IServices;
using Entities.Templates;

namespace Domain.Services;

public class ProxyApiClient: IProxyApiClient
{
    private readonly HttpClient _httpClient;

    public ProxyApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<bool> CongratilationToProxyApiAsync(PozdrStringDTO pozdrik)
    {
        var response = await _httpClient.PostAsJsonAsync("api/neirogen/pozdrik/add", pozdrik);
        return response.IsSuccessStatusCode;
    }

    public async Task<AddIntAndPozhDto?> GetAddIntAndCongratilationAsync(PozdrikIdDto pozdrikId)
    {
        var response = await _httpClient.GetAsync($"api/neirogen/pozdrik/{pozdrikId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error fetching Name: {response.ReasonPhrase}");
        }

        return await response.Content.ReadFromJsonAsync<AddIntAndPozhDto>();
    }

    public async Task<string?> GetName(PozdrikIdDto pozdrikId)
    {
        var response = await _httpClient.GetAsync($"api/neirogen/pozdrik/name/{pozdrikId}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error fetching UserName: {response.ReasonPhrase}");
        }
        var result = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(result))
        {
            response = await _httpClient.GetAsync($"api/neirogen/pozdrik/username/{pozdrikId}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error fetching UserName: {response.ReasonPhrase}");
            }
            result = await response.Content.ReadAsStringAsync();
        }
        
        return result;
    }
}