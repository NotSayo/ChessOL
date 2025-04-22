using Shared.Chess.Pieces;
using Shared.Chess.SimplePieces;
using Shared.Types;

namespace Shared.Chess.GameManager;

public class GameInfo
{
    public required EPieceColor CurrentTurn { get; set; }
    public required List<SimplePiece> Pieces { get; set; }
}