using System.Drawing;

namespace ChessApp.WebSockets;

public static class WebSocketMessageTranslator
{
    public static IWebSocketMessage? FromBytes(byte[] bytes)
    {
        if (bytes.Length < 1 || bytes[0] == 0)
            return null;

        if (bytes[0] == 1) // move
        {
            return new WebSocketMoveMessage(
                bytes[1],
                bytes[2],
                bytes[3],
                bytes[4]
            );
        }

        if (bytes[0] == 2) // game start
        {
            char[] ratingChars = new char[4];
            Array.Copy(bytes, 2, ratingChars, 0, 4);
            string ratingString = new string(ratingChars);
            int rating = Int32.Parse(ratingString);

            int length = bytes[5];
            char[] nameChars = new char[length];
            Array.Copy(bytes, 6, nameChars, 0, length);
            string nameString = new string(nameChars);

            return new WebSocketGameStartedMessage(
                bytes[1],
                nameString,
                rating
            );
        }

        if (bytes[0] == 3) // game over
        {
            return new WebSocketGameOverMessage(
                bytes[1]
            );
        }

        if (bytes[0] == 4) // resign
        {
            return new WebSocketResignMessage();
        }

        return null;
    }

    public static byte[] ToBytes(IWebSocketMessage webSocketMessage)
    {
        if (webSocketMessage is WebSocketMoveMessage moveMessage)
        {
            return new[]
            {
                (byte)1,
                (byte)moveMessage.ColFrom,
                (byte)moveMessage.RowFrom,
                (byte)moveMessage.ColTo,
                (byte)moveMessage.RowTo
            };
        }

        if (webSocketMessage is WebSocketGameStartedMessage gameStartedMessage)
        {
            byte[] bytes = new byte[7 + gameStartedMessage.OpponentName.Length];

            bytes[0] = (byte)2;
            bytes[1] = (byte)gameStartedMessage.Color;
            if (gameStartedMessage.OpponentRating >= 1000)
            {
                bytes[2] = (byte)gameStartedMessage.OpponentRating.ToString().ToCharArray()[0];
                bytes[3] = (byte)gameStartedMessage.OpponentRating.ToString().ToCharArray()[1];
                bytes[4] = (byte)gameStartedMessage.OpponentRating.ToString().ToCharArray()[2];
                bytes[5] = (byte)gameStartedMessage.OpponentRating.ToString().ToCharArray()[3];
            }
            else
            {
                bytes[2] = (byte)'0';
                bytes[3] = (byte)gameStartedMessage.OpponentRating.ToString().ToCharArray()[0];
                bytes[4] = (byte)gameStartedMessage.OpponentRating.ToString().ToCharArray()[1];
                bytes[5] = (byte)gameStartedMessage.OpponentRating.ToString().ToCharArray()[2];
            }

            bytes[6] = (byte)gameStartedMessage.OpponentName.Length;

            for (int i = 7; i < 7 + gameStartedMessage.OpponentName.Length; i++)
            {
                bytes[i] = (byte)gameStartedMessage.OpponentName[i - 7];
            }

            return bytes;
        }

        if (webSocketMessage is WebSocketGameOverMessage gameOverMessage)
        {
            return new[]
            {
                (byte)3,
                (byte)gameOverMessage.ColorWon
            };
        }

        if (webSocketMessage is WebSocketResignMessage resignMessage)
        {
            return new[]
            {
                (byte)4
            };
        }

        return new byte[1];
    }
}