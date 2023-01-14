using ChessApp.Api;
using Microsoft.AspNetCore.Mvc;

namespace ChessAppServer.Controllers;

[Route("/health")]
public class HealthController: ControllerBase
{
    [HttpGet]
    public HealthCheckResponse GetStatus()
    {
        return HealthCheckResponse.OK();
    }
}