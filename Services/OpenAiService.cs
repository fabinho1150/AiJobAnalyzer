using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class OpenAiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public OpenAiService(IConfiguration config, IHttpClientFactory factory)
    {
        _apiKey = config["OpenAI:ApiKey"] ?? "";
        _httpClient = factory.CreateClient();
    }

    public async Task<(string result, bool isDemo)> AnalyzeJobAsync(string jobText)
    {
        try
        {
            var requestBody = new
            {
                model = "gpt-4.1-mini",
                input = $"Analysiere diese Stellenanzeige:\n{jobText}"
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.PostAsync(
                "https://api.openai.com/v1/responses",
                content);

            if (!response.IsSuccessStatusCode)
                return (GetDemoResponse(), true);

            var responseString = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseString);

            var text = doc.RootElement
                .GetProperty("output")[0]
                .GetProperty("content")[0]
                .GetProperty("text")
                .GetString();

            return (text ?? GetDemoResponse(), false);
        }
        catch
        {
            return (GetDemoResponse(), true);
        }
    }

    private string GetDemoResponse()
    {
        return """
Zusammenfassung:
Diese Stelle richtet sich an einen Junior .NET Entwickler im Webbereich.

Wichtige Skills:
- C#
- ASP.NET Core
- REST APIs
- SQL

Match Score:
85 / 100

Tipps:
- Zeige GitHub Projekte
- Betone praktische Erfahrung
- Hebe deine Lernbereitschaft hervor
""";
    }
}
