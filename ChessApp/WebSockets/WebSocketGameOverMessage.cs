namespace ChessApp.WebSockets;

public class WebSocketGameOverMessage: IWebSocketMessage
{
    public int ColorWon;

    public WebSocketGameOverMessage(int colorWon)
    {
        ColorWon = colorWon;
    }
}