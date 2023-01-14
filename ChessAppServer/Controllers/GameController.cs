using System.Net.WebSockets;
using ChessAppServer.Persistence;
using ChessAppServer.Persistence.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ChessAppServer.Controllers;

[Route("/game")]
public class GameController: ControllerBase
{
    private readonly DataContext _context;
    private readonly ILogger<UserController> _logger;

    public GameController(ILogger<UserController> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("/{userId:long}")]
    public async Task New(long userId)
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            User? user = _context.User.Find(userId);
            
            var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            if (webSocket != null && webSocket.State == WebSocketState.Open)
            {
                while (true)
                {
                    var response = $"Hello! Time {DateTime.Now.ToString()}";
                    var bytes = System.Text.Encoding.UTF8.GetBytes(response);

                    await webSocket.SendAsync(new ArraySegment<byte>(bytes),
                        WebSocketMessageType.Text, true, CancellationToken.None);

                    await Task.Delay(2000);
                }
            }
        }
    }
}