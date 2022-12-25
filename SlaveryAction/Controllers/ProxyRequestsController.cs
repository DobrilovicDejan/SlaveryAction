using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;

namespace SlaveryAction.Controllers;

[ApiController]
public class ProxyRequests : ControllerBase
{
    private readonly bool _status = false; 
    
    [HttpGet("/proxy/status", Name = "Get the status of the proxy server.")]
    public IActionResult Status()
    {
        var jsonObject = new JsonObject
        {
            { "status", _status ? "running" : "inactive" }
        };

        return Ok(jsonObject);
    }

    [HttpPost("/proxy/request", Name = "Request a http request from the proxy server.")]
    public IActionResult NewRequest(string url, string? type, string? body)
    {
        if (url.Length < 2)
            return GenerateResult(false, "You did not provide the url for the request.");

        type ??= "get";

        var response = SendRequest(url, type, body);
        
        var responseString = response.Result.Content.ReadAsStringAsync().Result;

        return GenerateResult(response.Result.IsSuccessStatusCode, responseString);
    }

    private async Task<HttpResponseMessage> SendRequest(string url, string type, string? body)
    {
        var client = new HttpClient();

        switch (type.ToLower())
        {
            case "get":
                return await client.GetAsync(url);
            default:
                return await client.PostAsync(url, null);
        }
    }
    private IActionResult GenerateResult(bool success, JsonNode information)
    {
        var jsonObject = new JsonObject
        {
            { "status", success ? "success" : "error" },
            { "information", information }
        };
        return StatusCode(success ? 200 : 400, jsonObject);
    }
}