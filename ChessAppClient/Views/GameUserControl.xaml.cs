using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChessApp.Chess;
using ChessApp.Chess.Pieces;
using ChessApp.WebSockets;
using ChessAppClient.Communication;
using ChessAppClient.ViewModels;
using ChessAppServer.Infrastructure;

namespace ChessAppClient;

public partial class GameUserControl : UserControl
{
    private ClientWebSocket? ws;
    
    private ChessBoard ChessBoard;
    private Piece? chosenPiece;
    
    private Piece.Color? playerColor;
    private Piece.Color? playerToMove;

    private bool inGame = false;

    private readonly Rectangle chosenPieceRect = new Rectangle();
    private readonly List<Rectangle> moveRects = new List<Rectangle>();

    public GameUserControl()
    {
        chosenPieceRect.Stroke = new SolidColorBrush(Colors.Red);
        chosenPieceRect.StrokeThickness = 2;
        InitializeComponent();
        InitializeChessBoard();
        YourLabel.Content = $"{UserHolder.Username} ({UserHolder.Rating})";
    }

    private void InitializeChessBoard()
    {
        Color color1 = Colors.BurlyWood;
        Color color2 = Colors.Green;
        
        Color currentColor = color1;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Rectangle rectangle = new Rectangle
                {
                    Fill = new SolidColorBrush(currentColor)
                };
                Grid.SetRow(rectangle, i);
                Grid.SetColumn(rectangle, j);

                ChessBoardGrid.Children.Add(rectangle);

                currentColor = currentColor == color1 ? color2 : color1;
            }
            currentColor = currentColor == color1 ? color2 : color1;
        }
    }

    private void InitializeChessPieces()
    {
        ChessBoard = new ChessBoard();

        var images = ChessBoardGrid.Children
            .Cast<UIElement>()
            .OfType<Image>();
        
        images.ToList()
            .ForEach(image => ChessBoardGrid.Children.Remove(image));

        if (playerColor == Piece.Color.WHITE)
            ChessBoardGrid.LayoutTransform = new MatrixTransform(1, 0, 0, -1, 0, 0);

        if (playerColor == Piece.Color.BLACK)
            ChessBoardGrid.LayoutTransform = Transform.Identity;
        
        for (int i = 0; i < 8; i++)
        {
            AddPieceImage(0, i, ChessBoard.squares[0, i].GetImageFileName());
            AddPieceImage(1, i, ChessBoard.squares[1, i].GetImageFileName());
            AddPieceImage(6, i, ChessBoard.squares[6, i].GetImageFileName());
            AddPieceImage(7, i, ChessBoard.squares[7, i].GetImageFileName());
        }
    }

    private void AddPieceImage(int row, int col, string name)
    {
        Image image = new Image();
        image.Stretch = Stretch.Uniform;
        image.Height = 80;
        
        if (playerColor == Piece.Color.WHITE)
            image.LayoutTransform = new MatrixTransform(1, 0, 0, -1, 0, 0);
        
        BitmapImage bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.UriSource = GetUri("Images/" + name);
        bitmapImage.EndInit();
        image.Source = bitmapImage;

        Grid.SetRow(image, row);
        Grid.SetColumn(image, col);
        ChessBoardGrid.Children.Add(image);
    }

    private static Uri GetUri(string resourcePath)
    {
        var uri = string.Format(
            "pack://application:,,,/{0};component/{1}"
            , Assembly.GetExecutingAssembly().GetName().Name
            , resourcePath
        );
        return new Uri(uri);
    }

    private async void ChessBoardGrid_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (playerToMove != playerColor)
            return;

        Point position = e.GetPosition(sender as IInputElement);
        int clickedCol = (int)(position.X / 100);
        int clickedRow = (int)(position.Y / 100);
        
        if (ChessBoard.squares[clickedRow, clickedCol] != null && ChessBoard.squares[clickedRow, clickedCol].color == playerColor)
        {
            chosenPiece = ChessBoard.squares[clickedRow, clickedCol];
            Grid.SetColumn(chosenPieceRect, clickedCol);
            Grid.SetRow(chosenPieceRect, clickedRow);
            if (!ChessBoardGrid.Children.Contains(chosenPieceRect))
                ChessBoardGrid.Children.Add(chosenPieceRect);

            List<Move> moves = ChessBoard.GetMoves(clickedRow, clickedCol)!;
            
            moveRects.ForEach(rect => ChessBoardGrid.Children.Remove(rect));

            moves.ForEach(move =>
            {
                Rectangle rect = new Rectangle();
                rect.Stroke = new SolidColorBrush(Colors.Yellow);
                rect.StrokeThickness = 2;
                Grid.SetColumn(rect, move.ColTo);
                Grid.SetRow(rect, move.RowTo);
                ChessBoardGrid.Children.Add(rect);
                moveRects.Add(rect);
            });
            
            return;
        }

        if (chosenPiece != null && (chosenPiece.col != clickedCol || chosenPiece.row != clickedRow))
        {
            int chosenPieceCol = chosenPiece.col;
            int chosenPieceRow = chosenPiece.row;

            bool moveAllowed = ChessBoard.MakeMove(chosenPiece, clickedCol, clickedRow);

            if (moveAllowed)
            {
                var moveMessage = new WebSocketMoveMessage(chosenPieceCol, chosenPieceRow, clickedCol, clickedRow);
                await ws.SendAsync(new ArraySegment<byte>(WebSocketMessageTranslator.ToBytes(moveMessage)),
                    WebSocketMessageType.Text, true, CancellationToken.None);
                
                VisualizeMove(chosenPieceCol, chosenPieceRow, clickedCol, clickedRow);
                
                moveRects.ForEach(rect => ChessBoardGrid.Children.Remove(rect));
                ChessBoardGrid.Children.Remove(chosenPieceRect);

                chosenPiece = null;
                SwitchPlayerToMove();
            }
        }
    }

    private void VisualizeMove(int chosenPieceCol, int chosenPieceRow, int clickedCol, int clickedRow)
    {
        Image imageToMove = ChessBoardGrid.Children
            .Cast<UIElement>()
            .OfType<Image>()
            .FirstOrDefault(image =>
                Grid.GetColumn(image) == chosenPieceCol && Grid.GetRow(image) == chosenPieceRow)!;

        Image? imageOnDestination = ChessBoardGrid.Children
            .Cast<UIElement>()
            .OfType<Image>()
            .FirstOrDefault(image =>
                Grid.GetColumn(image) == clickedCol && Grid.GetRow(image) == clickedRow);

        if (chosenPieceCol != clickedCol && ChessBoard.squares[clickedRow, clickedCol] != null && ChessBoard.squares[clickedRow, clickedCol] is Pawn pawn)
        {
            Image? enPassantedPawnImage = ChessBoardGrid.Children
                .Cast<UIElement>()
                .OfType<Image>()
                .FirstOrDefault(image =>
                    Grid.GetColumn(image) == clickedCol && Grid.GetRow(image) == chosenPieceRow);
            if (ChessBoard.squares[chosenPieceRow, clickedCol] == null && enPassantedPawnImage != null)
            {
                ChessBoardGrid.Children.Remove(enPassantedPawnImage);
            }
        }

        if (ChessBoard.squares[clickedRow, clickedCol] is Queen queen && clickedRow is 0 or 7)
        {
            ChessBoardGrid.Children.Remove(imageToMove);
            AddPieceImage(clickedRow, clickedCol, ChessBoard.squares[clickedRow, clickedCol].GetImageFileName());
        }

        if (ChessBoard.squares[clickedRow, clickedCol] is King king)
        {
            Image? castledRook = ChessBoardGrid.Children
                .Cast<UIElement>()
                .OfType<Image>()
                .FirstOrDefault(image =>
                    Grid.GetColumn(image) == 3 && Grid.GetRow(image) == king.row);
            
            if (ChessBoard.squares[king.row, 3] is Rook && castledRook == null)
            {
                castledRook = ChessBoardGrid.Children
                    .Cast<UIElement>()
                    .OfType<Image>()
                    .FirstOrDefault(image =>
                        Grid.GetColumn(image) == 0 && Grid.GetRow(image) == king.row);
                Grid.SetColumn(castledRook, 3);
            }
            
            castledRook = ChessBoardGrid.Children
                .Cast<UIElement>()
                .OfType<Image>()
                .FirstOrDefault(image =>
                    Grid.GetColumn(image) == 5 && Grid.GetRow(image) == king.row);
            
            if (ChessBoard.squares[king.row, 5] is Rook && castledRook == null)
            {
                castledRook = ChessBoardGrid.Children
                    .Cast<UIElement>()
                    .OfType<Image>()
                    .FirstOrDefault(image =>
                        Grid.GetColumn(image) == 7 && Grid.GetRow(image) == king.row);
                Grid.SetColumn(castledRook, 5);
            }
        }

        Grid.SetColumn(imageToMove, clickedCol);
        Grid.SetRow(imageToMove, clickedRow);

        if (imageOnDestination != null)
            ChessBoardGrid.Children.Remove(imageOnDestination);
    }

    private async void FindGame_OnClick(object sender, RoutedEventArgs e)
    {
        if (inGame)
        {
            var resignMessage = new WebSocketResignMessage();
            await ws.SendAsync(new ArraySegment<byte>(WebSocketMessageTranslator.ToBytes(resignMessage)),
                WebSocketMessageType.Text, true, CancellationToken.None);
            
            return;
        }
        
        using (ws = new ClientWebSocket())
        {
            string hostAddress = RequestHandler.HostAddress;
            await ws.ConnectAsync(new Uri($"ws://{hostAddress}/game/{UserHolder.Id}"), CancellationToken.None);
            var buffer = new byte[256];
            while (ws.State == WebSocketState.Open)
            {
                for (int i = 0; i < 256; i++)
                    buffer[i] = 0;
                var result = await ws.ReceiveAsync(buffer, CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
                }
                else
                {
                    var webSocketMessage = WebSocketMessageTranslator.FromBytes(buffer);

                    if (webSocketMessage is WebSocketGameStartedMessage gameStartedMessage)
                    {
                        FindGameButton.Content = "Resign";
                        inGame = true;
                        playerColor = gameStartedMessage.Color == 0 ? Piece.Color.WHITE : Piece.Color.BLACK;
                        playerToMove = Piece.Color.WHITE;
                        OpponentLabel.Content = gameStartedMessage.OpponentName + $" ({gameStartedMessage.OpponentRating})";
                        UserHolder.OpponentRating = gameStartedMessage.OpponentRating;
                        InitializeChessPieces();
                    }
                    
                    if (webSocketMessage is WebSocketMoveMessage moveMessage)
                    {
                        Piece? pieceToMove = ChessBoard.squares[moveMessage.RowFrom, moveMessage.ColFrom];

                        ChessBoard.MakeMove(pieceToMove, moveMessage.ColTo, moveMessage.RowTo);
                        
                        VisualizeMove(moveMessage.ColFrom, moveMessage.RowFrom, moveMessage.ColTo, moveMessage.RowTo);
                        SwitchPlayerToMove();
                    }

                    if (webSocketMessage is WebSocketGameOverMessage gameOverMessage)
                    {
                        FindGameButton.Content = "Find Game";
                        inGame = false;
                        var colorWon = gameOverMessage.ColorWon == 0 ? "WHITE" : "BLACK" ;
                        
                        bool isWon = gameOverMessage.ColorWon == 0
                            ? playerColor == Piece.Color.WHITE
                            : playerColor == Piece.Color.BLACK;
                        int ratingChange = RatingChangeCalculator.ratingChange(UserHolder.Rating, UserHolder.OpponentRating, isWon);
                        UserHolder.Rating += ratingChange;
                        YourLabel.Content = $"{UserHolder.Username} ({UserHolder.Rating})";

                        if (gameOverMessage.ColorWon == 2)
                        {
                            MessageBox.Show(
                                $"Game over!",
                                "Game is a draw!",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information,
                                MessageBoxResult.None
                            );
                        }
                        else
                        {
                            MessageBox.Show(
                                $"Game over, {colorWon} won!",
                                "Game Over!",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information,
                                MessageBoxResult.None
                            );
                        }
                    }
                }
            }
        }
    }

    private void SwitchPlayerToMove()
    {
        playerToMove = playerToMove == Piece.Color.WHITE ? Piece.Color.BLACK : Piece.Color.WHITE;
    }
}