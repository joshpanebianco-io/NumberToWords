using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace NumberToWords.Tests;

public class ApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Valid_number_returns_words()
    {
        var response = await _client.GetAsync("/api/convert?number=123.45");
        var body = await response.Content.ReadFromJsonAsync<ConvertResponse>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS", body?.Words);
    }

    [Theory]
    [InlineData("/api/convert?number=abc")]
    [InlineData("/api/convert?number=")]
    [InlineData("/api/convert")]
    public async Task Invalid_number_returns_bad_request(string url)
    {
        var response = await _client.GetAsync(url);
        var body = await response.Content.ReadFromJsonAsync<ConvertResponse>();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.False(string.IsNullOrEmpty(body?.Error));
    }

    private record ConvertResponse(string? Words, string? Error);
}
