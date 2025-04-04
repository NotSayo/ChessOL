﻿using MudBlazor;
using Shared.Chess.Pieces;
using Shared.Types;

namespace Shared.Chess.GameManager;

public class GameInstance
{
    public List<IPiece> Pieces { get; set; }
    public List<IPiece> TakenPiecesBlack { get; set; }
    public List<IPiece> TakenPiecesWhite { get; set; }
    public IPiece?[,] Board = new IPiece[8,8];

    public void InitializeGame()
{
    Pieces.Add(new Rook()
        { GameInstance = this, Icon = Icons.Custom.Uncategorized.ChessRook, PieceColor = EPieceColor.White, Repetitive = true, Position = new() { Y = 7, X = 0 } });
    Pieces.Add(new Rook()
        { GameInstance = this, Icon = Icons.Custom.Uncategorized.ChessRook, PieceColor = EPieceColor.White, Repetitive = true, Position = new() { Y = 7, X = 7 } });
    Pieces.Add(new Rook()
        { GameInstance = this, Icon = Icons.Custom.Uncategorized.ChessRook, PieceColor = EPieceColor.Black, Repetitive = true, Position = new() { Y = 0, X = 0 } });
    Pieces.Add(new Rook()
        { GameInstance = this, Icon = Icons.Custom.Uncategorized.ChessRook, PieceColor = EPieceColor.Black, Repetitive = true, Position = new() { Y = 0, X = 7 } });

    Pieces.Add(new Knight()
        { GameInstance = this, Icon = Icons.Custom.Uncategorized.ChessKnight, PieceColor = EPieceColor.White, Repetitive = false, Position = new() { Y = 7, X = 1 } });
    Pieces.Add(new Knight()
        { GameInstance = this, Icon = Icons.Custom.Uncategorized.ChessKnight, PieceColor = EPieceColor.White, Repetitive = false, Position = new() { Y = 7, X = 6 } });
    Pieces.Add(new Knight()
        { GameInstance = this, Icon = Icons.Custom.Uncategorized.ChessKnight, PieceColor = EPieceColor.Black, Repetitive = false, Position = new() { Y = 0, X = 1 } });
    Pieces.Add(new Knight()
        { GameInstance = this, Icon = Icons.Custom.Uncategorized.ChessKnight, PieceColor = EPieceColor.Black, Repetitive = false, Position = new() { Y = 0, X = 6 } });

    Pieces.Add(new Bishop()
        { GameInstance = this, Icon = Icons.Custom.Uncategorized.ChessBishop, PieceColor = EPieceColor.White, Repetitive = true, Position = new() { Y = 7, X = 2 } });
    Pieces.Add(new Bishop()
        { GameInstance = this, Icon = Icons.Custom.Uncategorized.ChessBishop, PieceColor = EPieceColor.White, Repetitive = true, Position = new() { Y = 7, X = 5 } });
    Pieces.Add(new Bishop()
        { GameInstance = this, Icon = Icons.Custom.Uncategorized.ChessBishop, PieceColor = EPieceColor.Black, Repetitive = true, Position = new() { Y = 0, X = 2 } });
    Pieces.Add(new Bishop()
        { GameInstance = this, Icon = Icons.Custom.Uncategorized.ChessBishop, PieceColor = EPieceColor.Black, Repetitive = true, Position = new() { Y = 0, X = 5 } });

    Pieces.Add(new Queen()
        { GameInstance = this, Icon = Icons.Custom.Uncategorized.ChessQueen, PieceColor = EPieceColor.White, Repetitive = true, Position = new() { Y = 7, X = 3 } });
    Pieces.Add(new Queen()
        { GameInstance = this, Icon = Icons.Custom.Uncategorized.ChessQueen, PieceColor = EPieceColor.Black, Repetitive = true, Position = new() { Y = 0, X = 3 } });

    Pieces.Add(new King()
        { GameInstance = this, Icon = Icons.Custom.Uncategorized.ChessKing, PieceColor = EPieceColor.White, Repetitive = false, Position = new() { Y = 7, X = 4 } });
    Pieces.Add(new King()
        { GameInstance = this, Icon = Icons.Custom.Uncategorized.ChessKing, PieceColor = EPieceColor.Black, Repetitive = false, Position = new() { Y = 0, X = 4 } });

    for (int i = 0; i < 8; i++)
    {
        Pieces.Add(new Pawn(EPieceColor.White)
            { GameInstance = this, Icon = Icons.Custom.Uncategorized.ChessPawn, Repetitive = false, Position = new() { Y = 6, X = i } });
        Pieces.Add(new Pawn(EPieceColor.Black)
            { GameInstance = this, Icon = Icons.Custom.Uncategorized.ChessPawn, Repetitive = false, Position = new() { Y = 1, X = i }, IsLastMoveDouble = true }); // TODO: Fix this
    }

    foreach (var piece in Pieces)
    {
        Board[piece.Position.Y, piece.Position.X] = piece;
    }
}


    public void RemoveFromBoard(IPiece piece)
    {
        Console.WriteLine($"{Board[piece.Position.Y, piece.Position.X].Position.X} - {Board[piece.Position.Y, piece.Position.X].Position.Y}");

        Board[piece.Position.Y, piece.Position.X] = null;
    }
    
    public void AddToBoard(IPiece piece)
    {
        Board[piece.Position.Y, piece.Position.X] = piece;

        Console.WriteLine($"{Board[piece.Position.Y, piece.Position.X].Position.X} - {Board[piece.Position.Y, piece.Position.X].Position.Y}");
    }

    public void TakePiece(IPiece piece)
    {
        Board[piece.Position.Y, piece.Position.X] = null;
        if (piece.PieceColor == EPieceColor.Black)
            TakenPiecesBlack.Add(piece);
        else
            TakenPiecesWhite.Add(piece);
    }
}