using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CountriesApi.Controllers;

[ApiController]
[Route("countries")]
public class CountriesController : ControllerBase
{
    private readonly HttpClient _client;

    public CountriesController()
    {
        _client = new HttpClient();
    }

    [HttpGet("{country}")]
    public async Task<ActionResult<List<University>>> Get([FromRoute] string country)
    {
        await Task.Delay(5000);
        var response = await _client.GetAsync("http://universities.hipolabs.com/search?country=" + country);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<List<University>>(json)
            ?? throw new JsonSerializationException($"Error deserializing universities data.");

        return data;
    }
}

public class University
{
    public string Name { get; set; }

    [JsonProperty("state-province")]
    public string StateProvince { get; set; }

    public string Country { get; set; }

    [JsonProperty("alpha_two_code")]
    public string Code { get; set; }

    public string? WebPage => WebPages.FirstOrDefault();

    [JsonProperty("web_pages")]
    public List<string> WebPages { get; set; }

    public List<string> Domains { get; set; }
}
