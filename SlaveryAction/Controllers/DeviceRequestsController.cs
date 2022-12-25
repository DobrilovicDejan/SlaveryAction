using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;

namespace SlaveryAction.Controllers;

[ApiController]
public class DeviceRequests : ControllerBase
{
    private readonly bool _status = false; 
    
    [HttpGet("/device/status", Name = "Get the status of the device server.")]
    public IActionResult Status()
    {
        var jsonObject = new JsonObject
        {
            { "status", _status ? "running" : "inactive" }
        };

        return Ok(jsonObject);
    }

    [HttpPost("/device/request", Name = "Request a http request from the device server.")]
    public IActionResult NewRequest(string url, RequestType tip_trazenja, string? body)
    {
        if (url.Length < 2)
            return GenerateResult(false, "You did not provide the url for the request.");
        

        var response = SendRequest(url, tip_trazenja, body);
        
        var responseString = response.Result.Content.ReadAsStringAsync().Result;

        return GenerateResult(response.Result.IsSuccessStatusCode, responseString);
    }

    private async Task<HttpResponseMessage> SendRequest(string url, RequestType tip_trazenja, string? body)
    {
        var client = new HttpClient();
        
        return await client.PostAsync(url, null);
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