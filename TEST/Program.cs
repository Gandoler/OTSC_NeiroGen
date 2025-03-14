using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

class Program
{
    private static readonly string apiKey = "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"; // Вставь свой API-ключ
    private static readonly string apiUrl = "https://openrouter.ai/api/v1/chat/completions";

    static async Task Main()
    {
        var userMessage = "Расскажи интересный факт про космос";
        
        var requestBody = new
        {
            model = "qwen/qwen2.5-vl-72b-instruct",
            messages = new[]
            {
                new { role = "system", content = "Ты помощник, отвечай кратко и точно." },
                new { role = "user", content = userMessage }
            }
        };

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                string responseJson = await response.Content.ReadAsStringAsync();

                // Разбираем JSON и получаем только ответ нейросети
                var jsonNode = JsonNode.Parse(responseJson);
                var answer = jsonNode?["choices"]?[0]?["message"]?["content"]?.ToString();

                Console.WriteLine("Ответ нейросети: " + answer);
            }
            else
            {
                Console.WriteLine($"Ошибка {response.StatusCode}: {await response.Content.ReadAsStringAsync()}");
            }
        }
    }
}