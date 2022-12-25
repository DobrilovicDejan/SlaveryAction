using SlaveryAction.Controllers;

namespace SlaveryAction;

public class Device
{
    public int dev_id { get; set; }

    public RequestType tip_potrage { get; set; }

    public string? dodatan_text { get; set; }

    public Device(int devId, string? dodatanText)
    {
        dev_id = devId;
        dodatan_text = dodatanText;
    }

    public bool newRequest(RequestType tip_potrage)
    {
        string url = "http://localhost:5248/device/request";
        bool suck = false;
        try
        {
            DeviceRequests devReq = new DeviceRequests();
            devReq.NewRequest(url, tip_potrage, dodatan_text);
            suck = true;
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return suck;

    }
}