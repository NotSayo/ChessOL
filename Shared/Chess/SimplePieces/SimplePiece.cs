using MudBlazor;
using Shared.Types;

namespace Shared.Chess.SimplePieces;

public class SimplePiece
{
    public EPieceColor EPieceColor { get; set; }
    public EPieceType EPieceType { get; set; }
    public bool IsAlive { get; set; } = true;
    public string Icon { get; set; }

    public Vector Position { get; set; }

    public SimplePiece()
    {

    }

    public void AssignIcon()
    {
        Icon = EPieceType switch
        {
            EPieceType.King => Icons.Custom.Uncategorized.ChessKing,
            EPieceType.Queen => Icons.Custom.Uncategorized.ChessQueen,
            EPieceType.Bishop => Icons.Custom.Uncategorized.ChessBishop,
            EPieceType.Knight => Icons.Custom.Uncategorized.ChessKnight,
            EPieceType.Rook => Icons.Custom.Uncategorized.ChessRook,
            EPieceType.Pawn => Icons.Custom.Uncategorized.ChessPawn,
        };
    }
}