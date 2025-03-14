using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Domain.Services.IServices;

namespace Domain.Services;

public class GenerateCongratulations:IGenerateCongratilation
{
    private readonly string apiUrl;
    private readonly HttpClient _httpClient;

    public GenerateCongratulations(string? apiKey, string apiUrl, HttpClient httpClient)
    {
        this.apiUrl = apiUrl;
        this._httpClient = httpClient;
        this._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
    }

    public async Task<string?> GenerateAsync(string? name, string? interests, string? wishes)
    {
        string userMessage = $"Напиши персональное поздравление для {name}. У него(неё) " +
                             $"интересы: {(string.IsNullOrEmpty(interests) ? "машины" : interests)}. Он(она) " +
                             $"хочет: {(string.IsNullOrEmpty(wishes) ? "счастья и здоровья" : wishes)}. " +
                             "Поздравление должно быть теплым, душевным и личным, с учетом интересов и пожеланий.";

        var requestBody = new
        {
            model = "qwen/qwen2.5-vl-72b-instruct",
            temperature = 0.8,  
            messages = new[]
            {
                new { role = "system", content = "Ты профессиональный писатель поздравлений. Создавай уникальные и эмоциональные поздравления." },
                new { role = "user", content = userMessage }
            }
        };

        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, content);

        if (response.IsSuccessStatusCode)
        {
            string responseJson = await response.Content.ReadAsStringAsync();
            var jsonNode = JsonNode.Parse(responseJson);
            return jsonNode?["choices"]?[0]?["message"]?["content"]?.ToString();
        }

        return null;
    }
}