namespace ChessApp.WebSockets;

public class WebSocketGameStartedMessage: IWebSocketMessage
{
    public int Color;
    public String OpponentName;
    public int OpponentRating;

    public WebSocketGameStartedMessage(int color, string opponentName, int opponentRating)
    {
        Color = color;
        OpponentName = opponentName;
        OpponentRating = opponentRating;
    }
}