using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChessAppClient.Infrastructure.Chess;

namespace ChessAppClient;

public partial class GameUserControl : UserControl
{
    private ChessBoard ChessBoard;
    private Piece? chosenPiece;
    private Piece.Color? playerColor = Piece.Color.WHITE;

    public GameUserControl()
    {
        ChessBoard = new ChessBoard();
        InitializeComponent();
        InitializeChessBoard();
        InitializeChessPieces();
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
        ChessBoardGrid.LayoutTransform = new MatrixTransform(1, 0, 0, -1, 0, 0); // TODO omit if black
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
        image.LayoutTransform = new MatrixTransform(1, 0, 0, -1, 0, 0); // TODO omit if black
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

    private void ChessBoardGrid_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        Point position = e.GetPosition(sender as IInputElement);
        int clickedCol = (int)(position.X / 100);
        int clickedRow = (int)(position.Y / 100);
        
        if (chosenPiece == null && ChessBoard.squares[clickedRow, clickedCol] != null)
        {
            chosenPiece = ChessBoard.squares[clickedRow, clickedCol];
            return;
        }

        if (chosenPiece != null && (chosenPiece.col != clickedCol || chosenPiece.row != clickedRow))
        {
            int chosenPieceCol = chosenPiece.col;
            int chosenPieceRow = chosenPiece.row;

            bool moveAllowed = ChessBoard.MakeMove(chosenPiece, clickedCol, clickedRow);

            if (moveAllowed)
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

                Grid.SetColumn(imageToMove, clickedCol);
                Grid.SetRow(imageToMove, clickedRow);

                if (imageOnDestination != null)
                    ChessBoardGrid.Children.Remove(imageOnDestination);

                chosenPiece = null;
            }
        }
    }

    private void FindGame_OnClick(object sender, RoutedEventArgs e)
    {
        
    }
}