using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;

namespace SlaveryAction.Controllers;

[ApiController]
public class DeviceRequests : ControllerBase
{
    private readonly bool _status = false;
    private static RequestType _request { get; set; }
    
    [HttpGet("/device/status", Name = "Get the status of the device server.")]
    public IActionResult Status()
    {
        var jsonObject = new JsonObject
        {
            { "status", _status ? "running" : "inactive" }
        };

        return Ok(jsonObject);
    }
    [HttpGet("/device/request", Name = "Get the request of the device server.")]
    public IActionResult Request()
    {

        
        var jsonObject = new JsonObject
        {
            { "request", TypeToString(_request) }
        };

        return Ok(jsonObject);
    }
    

    [HttpPost("/device/request", Name = "Request a http request from the device server.")]
    public IActionResult NewRequest(string url, RequestType tip_trazenja, string? body)
    {
        if (url.Length < 2)
            return GenerateResult(false, tip_trazenja, "You did not provide the url for the request.");
        

        var response = SendRequest(url, tip_trazenja, body);
        
        var responseString = response.Result.Content.ReadAsStringAsync().Result;

        return GenerateResult(response.Result.IsSuccessStatusCode, tip_trazenja, responseString);
    }

    private async Task<HttpResponseMessage> SendRequest(string url, RequestType tip_trazenja, string? body)
    {
        var client = new HttpClient();
        _request = tip_trazenja;
        return await client.PostAsync(url, null);
    }
    private IActionResult GenerateResult(bool success, RequestType tip_trazenja, JsonNode information)
    {
       
        var jsonObject = new JsonObject
        {
            { "status", success ? "success" : "error" },
            { "information", information },
            { "request", TypeToString(_request)}
        };
        return StatusCode(success ? 200 : 400, jsonObject);
    }
    
    [ApiController]
    [Route("device")]
    public class DeviceRequestController : ControllerBase
    {
        [HttpGet(Name = "GetDeviceRequest")]
        public IActionResult Get()
        {
            var jsonObject = new JsonObject();
            jsonObject["status"] = "ok";
            jsonObject["request"] = TypeToString(_request);

            return Ok(jsonObject);
        }
    }
    
    
    
    public static string TypeToString(RequestType requestType)
    {
        string tipT;
        switch (requestType)
        {
            case RequestType.RawDataOf:
                tipT = "rawdataof";
                break;
            case RequestType.StructOf:
                tipT = "structof";
                break;
            case RequestType.NumberOf:
                tipT = "numberof";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(requestType), requestType, null);
        }

        return tipT;
    } 
    
}

