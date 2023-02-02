namespace ChessApp.WebSockets;

public class WebSocketMoveMessage: IWebSocketMessage
{

    public int ColFrom;
    public int RowFrom;

    public int ColTo;
    public int RowTo;

    public WebSocketMoveMessage(int colFrom, int rowFrom, int colTo, int rowTo)
    {
        ColFrom = colFrom;
        RowFrom = rowFrom;
        ColTo = colTo;
        RowTo = rowTo;
    }
}