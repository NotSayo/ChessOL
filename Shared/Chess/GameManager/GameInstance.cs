using MudBlazor;
using Shared.Chess.Pieces;
using Shared.Types;

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
            PieceColor = EPieceColor.White,
            Repetitive = true,
            Position = new() { X = 7, Y = 0 }
        });
        Pieces.Add(new Rook()
        {
            GameInstance = this,
            Icon = @Icons.Custom.Uncategorized.ChessRook,
            PieceColor = EPieceColor.White,
            Repetitive = true,
            Position = new () { X = 7, Y = 7 }
        });
        Pieces.Add(new Rook()
        {
            GameInstance = this,
            Icon = @Icons.Custom.Uncategorized.ChessRook,
            PieceColor = EPieceColor.Black,
            Repetitive = true,
            Position = new() { X = 0, Y = 0 }
        });
        Pieces.Add(new Rook()
        {
            GameInstance = this,
            Icon = @Icons.Custom.Uncategorized.ChessRook,
            PieceColor = EPieceColor.Black,
            Repetitive = true,
            Position = new () { X = 0, Y = 7 }
        });
        Pieces.Add(new Knight()
        {
            GameInstance = this,
            Icon = @Icons.Custom.Uncategorized.ChessKnight,
            PieceColor = EPieceColor.White,
            Repetitive = false,
            Position = new() { X = 7, Y = 1 }
        });
        Pieces.Add(new Knight()
        {
            GameInstance = this,
            Icon = @Icons.Custom.Uncategorized.ChessKnight,
            PieceColor = EPieceColor.White,
            Repetitive = false,
            Position = new () { X = 7, Y = 6 }
        });
        Pieces.Add(new Knight()
        {
            GameInstance = this,
            Icon = @Icons.Custom.Uncategorized.ChessKnight,
            PieceColor = EPieceColor.Black,
            Repetitive = false,
            Position = new() { X = 0, Y = 1 }
        });
        Pieces.Add(new Knight()
        {
            GameInstance = this,
            Icon = @Icons.Custom.Uncategorized.ChessKnight,
            PieceColor = EPieceColor.Black,
            Repetitive = false,
            Position = new () { X = 0, Y = 6 }
        });
        Pieces.Add(new Bishop()
        {
            GameInstance = this,
            Icon = @Icons.Custom.Uncategorized.ChessBishop,
            PieceColor = EPieceColor.White,
            Repetitive = true,
            Position = new() { X = 7, Y = 2 }
        });
        Pieces.Add(new Bishop()
        {
            GameInstance = this,
            Icon = @Icons.Custom.Uncategorized.ChessBishop,
            PieceColor = EPieceColor.White,
            Repetitive = true,
            Position = new () { X = 7, Y = 5 }
        });
        Pieces.Add(new Bishop()
        {
            GameInstance = this,
            Icon = @Icons.Custom.Uncategorized.ChessBishop,
            PieceColor = EPieceColor.Black,
            Repetitive = true,
            Position = new() { X = 0, Y = 2 }
        });
        Pieces.Add(new Bishop()
        {
            GameInstance = this,
            Icon = @Icons.Custom.Uncategorized.ChessBishop,
            PieceColor = EPieceColor.Black,
            Repetitive = true,
            Position = new () { X = 0, Y = 5 }
        });
        Pieces.Add(new Queen()
        {
            GameInstance = this,
            Icon = @Icons.Custom.Uncategorized.ChessQueen,
            PieceColor = EPieceColor.White,
            Repetitive = true,
            Position = new() { X = 7, Y = 3 }
        });
        Pieces.Add(new Queen()
        {
            GameInstance = this,
            Icon = @Icons.Custom.Uncategorized.ChessQueen,
            PieceColor = EPieceColor.Black,
            Repetitive = true,
            Position = new () { X = 0, Y = 3 }
        });
        Pieces.Add(new King()
        {
            GameInstance = this,
            Icon = @Icons.Custom.Uncategorized.ChessKing,
            PieceColor = EPieceColor.White,
            Repetitive = false,
            Position = new() { X = 7, Y = 4 }
        });
        Pieces.Add(new King()
        {
            GameInstance = this,
            Icon = @Icons.Custom.Uncategorized.ChessKing,
            PieceColor = EPieceColor.Black,
            Repetitive = false,
            Position = new () { X = 0, Y = 4 }
        });
        for (int i = 0; i < 8; i++)
        {
            Pieces.Add(new Pawn()
            {
                GameInstance = this,
                Icon = @Icons.Custom.Uncategorized.ChessPawn,
                PieceColor = EPieceColor.White,
                Repetitive = false,
                Position = new () { X = 6, Y = i }
            });
        }
        for (int i = 0; i < 8; i++)
        {
            Pieces.Add(new Pawn()
            {
                GameInstance = this,
                Icon = @Icons.Custom.Uncategorized.ChessPawn,
                PieceColor = EPieceColor.Black,
                Repetitive = false,
                Position = new () { X = 1, Y = i }
            });
        }
    }
}