namespace SlaveryAction;

public class GeneralUse
{
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