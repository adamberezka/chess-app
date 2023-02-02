using System.Net.WebSockets;

namespace ChessAppServer.Infrastructure;

public class Player
{
    public long Id;
    public string Username;
    public int Rating;
    public WebSocket WebSocketConnection;

    public Player(long id, string username, int rating, WebSocket webSocketConnection)
    {
        Id = id;
        Username = username;
        Rating = rating;
        WebSocketConnection = webSocketConnection;
    }
}