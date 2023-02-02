using System.Net.WebSockets;
using ChessAppServer.Infrastructure;
using ChessAppServer.Persistence;
using ChessAppServer.Persistence.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ChessAppServer.Controllers;

public class GameController: ControllerBase
{
    private readonly DataContext _context;
    private readonly ILogger<UserController> _logger;
    private readonly GamesProcessor _gamesProcessor;

    public GameController(ILogger<UserController> logger, DataContext context, GamesProcessor gamesProcessor)
    {
        _logger = logger;
        _context = context;
        _gamesProcessor = gamesProcessor;
    }

    
    [HttpGet("/game/{userId:long}")]
    public async Task<IActionResult> New(long userId)
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            User? user = await _context.User.FindAsync(userId);

            var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            
            Player player = new Player(user.Id,
                user.Username, 
                user.Rating,
                webSocket);

            await Task.Run(() =>_gamesProcessor.AddPlayer(player, _context));

            while (webSocket.State == WebSocketState.Open)
            {
                
            }
        }

        return new StatusCodeResult(101);
    }
}