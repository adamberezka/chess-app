using System.Text.Json.Serialization;

namespace ChessApp.Api;

public class HealthCheckResponse
{
    public string status { get; set; }
    
    public static string STATUS_OK = "ChessAppHealthOK";
    public static HealthCheckResponse OK()
    {
        return new HealthCheckResponse(STATUS_OK);
    }

    public HealthCheckResponse(string status)
    {
        this.status = status;
    }
}