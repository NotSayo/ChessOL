using MudBlazor;
using Shared.Chess.Pieces;

namespace Shared.Chess.GameManager;

public class GameInstance
{
    public List<IPiece> Pieces { get; set; }
    public IPiece?[,] Board = new IPiece[8,8];

    public void InitializeGame()
    {
        Pieces.Add(new Rook()
        {
            GameInstance = this,
            Icon = @Icons.Custom.Uncategorized.ChessRook,
            PieceColor = "#FFFFFF",
            Repetitive = true,
            Identifier = "70",
            Position = new() { X = 7, Y = 0 }
        });
        Pieces.Add(new Rook()
        {
            GameInstance = this,
            Icon = @Icons.Custom.Uncategorized.ChessRook,
            PieceColor = "#FFFFFF",
            Repetitive = true,
            Identifier = "77",
            Position = new () { X = 7, Y = 7 }
        });
        Pieces.Add(new Rook()
        {
            GameInstance = this,
            Icon = @Icons.Custom.Uncategorized.ChessRook,
            PieceColor = "#000000",
            Repetitive = true,
            Identifier = "00",
            Position = new() { X = 0, Y = 0 }
        });
        Pieces.Add(new Rook()
        {
            GameInstance = this,
            Icon = @Icons.Custom.Uncategorized.ChessRook,
            PieceColor = "#000000",
            Repetitive = true,
            Identifier = "07",
            Position = new () { X = 0, Y = 7 }
        });
        Pieces.Add(new Knight()
        {
            GameInstance = this,
            Icon = @Icons.Custom.Uncategorized.ChessKnight,
            PieceColor = "#FFFFFF",
            Repetitive = false,
            Identifier = "71",
            Position = new() { X = 7, Y = 1 }
        });
        Pieces.Add(new Knight()
        {
            GameInstance = this,
            Icon = @Icons.Custom.Uncategorized.ChessKnight,
            PieceColor = "#FFFFFF",
            Repetitive = false,
            Identifier = "76",
            Position = new () { X = 7, Y = 6 }
        });
        Pieces.Add(new Knight()
        {
            GameInstance = this,
            Icon = @Icons.Custom.Uncategorized.ChessKnight,
            PieceColor = "#000000",
            Repetitive = false,
            Identifier = "01",
            Position = new() { X = 0, Y = 1 }
        });
        Pieces.Add(new Knight()
        {
            GameInstance = this,
            Icon = @Icons.Custom.Uncategorized.ChessKnight,
            PieceColor = "#000000",
            Repetitive = false,
            Identifier = "06",
            Position = new () { X = 0, Y = 6 }
        });
        Pieces.Add(new Bishop()
        {
            GameInstance = this,
            Icon = @Icons.Custom.Uncategorized.ChessBishop,
            PieceColor = "#FFFFFF",
            Repetitive = true,
            Identifier = "72",
            Position = new() { X = 7, Y = 2 }
        });
        Pieces.Add(new Bishop()
        {
            GameInstance = this,
            Icon = @Icons.Custom.Uncategorized.ChessBishop,
            PieceColor = "#FFFFFF",
            Repetitive = true,
            Identifier = "75",
            Position = new () { X = 7, Y = 5 }
        });
        Pieces.Add(new Bishop()
        {
            GameInstance = this,
            Icon = @Icons.Custom.Uncategorized.ChessBishop,
            PieceColor = "#000000",
            Repetitive = true,
            Identifier = "02",
            Position = new() { X = 0, Y = 2 }
        });
        Pieces.Add(new Bishop()
        {
            GameInstance = this,
            Icon = @Icons.Custom.Uncategorized.ChessBishop,
            PieceColor = "#000000",
            Repetitive = true,
            Identifier = "05",
            Position = new () { X = 0, Y = 5 }
        });
        Pieces.Add(new Queen()
        {
            GameInstance = this,
            Icon = @Icons.Custom.Uncategorized.ChessQueen,
            PieceColor = "#FFFFFF",
            Repetitive = true,
            Identifier = "73",
            Position = new() { X = 7, Y = 3 }
        });
        Pieces.Add(new Queen()
        {
            GameInstance = this,
            Icon = @Icons.Custom.Uncategorized.ChessQueen,
            PieceColor = "#000000",
            Repetitive = true,
            Identifier = "03",
            Position = new () { X = 0, Y = 3 }
        });
        Pieces.Add(new King()
        {
            GameInstance = this,
            Icon = @Icons.Custom.Uncategorized.ChessKing,
            PieceColor = "#FFFFFF",
            Repetitive = false,
            Identifier = "74",
            Position = new() { X = 7, Y = 4 }
        });
        Pieces.Add(new King()
        {
            GameInstance = this,
            Icon = @Icons.Custom.Uncategorized.ChessKing,
            PieceColor = "#000000",
            Repetitive = false,
            Identifier = "04",
            Position = new () { X = 0, Y = 4 }
        });
        for (int i = 0; i < 8; i++)
        {
            Pieces.Add(new Pawn()
            {
                GameInstance = this,
                Icon = @Icons.Custom.Uncategorized.ChessPawn,
                PieceColor = "#FFFFFF",
                Repetitive = false,
                Identifier = "6" + i,
                Position = new () { X = 6, Y = i }
            });
        }
        for (int i = 0; i < 8; i++)
        {
            Pieces.Add(new Pawn()
            {
                GameInstance = this,
                Icon = @Icons.Custom.Uncategorized.ChessPawn,
                PieceColor = "#000000",
                Repetitive = false,
                Identifier = "1" + i,
                Position = new () { X = 1, Y = i }
            });
        }
    }
}