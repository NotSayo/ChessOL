using Shared.Types;

namespace Shared.Chess.Pieces;

public class King : APiece
{
    public override List<Vector> Moves { get; set; } = new()
    {
        new Vector(1, 0),
        new Vector(1, 1),
        new Vector(0, 1),
        new Vector(-1, 1),
        new Vector(-1, 0),
        new Vector(-1, -1),
        new Vector(0, -1),
        new Vector(1, -1),
    };

    public override bool Repetitive { get; set; } = false;
    public bool IsInCheck { get; set; } = false;
    public bool IsInCheckmate { get; set; } = false;
    public bool IsInStalemate { get; set; } = false;

    public override void CheckAvailableMoves()
    {
        GameInstance.Pieces.Where(p => p != this).ToList().ForEach(p => p.CheckAvailableMoves());
        if (GameInstance.Pieces.Where(p => p.PieceColor != this.PieceColor)
            .Any(p => p.AvailableMoves.Any(m => m.Y == this.Position.Y && m.X == this.Position.X)))
        {
            Console.WriteLine("King is in Check");
            CheckAvailableMovesDuringCheck();
        }
        CheckAvailableMovesDuringCheck();
    }

    private void CheckAvailableMovesDuringCheck()
    {
        List<IPiece> checkingPieces = GameInstance.Pieces.Where(p => p.PieceColor != this.PieceColor)
            .Where(p => p.AvailableMoves.Any(m => m.Y == this.Position.Y && m.X == this.Position.X)).ToList();
        GameInstance.Pieces.Where(p => p.PieceColor == this.PieceColor).ToList().ForEach(p => p.AvailableMoves = new ());
        foreach (var vec in Moves)
        {
            if (CheckIfPositionIsAvailable(Position.Y + vec.Y, Position.X + vec.X))
            {
                AvailableMoves.Add(new Vector() {Y = Position.Y + vec.Y, X = Position.X + vec.X});
            }
        }
    }

    private bool CheckIfPositionIsAvailable(int y, int x)
    {
        try
        {
            if (GameInstance.Board[y, x] is not null)
            {
                var piece = GameInstance.Board[y, x];
                if (piece!.PieceColor != this.PieceColor)
                {
                    if (!GameInstance.Pieces.Where(p => p.PieceColor != this.PieceColor)
                            .Any(p => p.AvailableMoves.Any(m => m.Y == piece.Position.Y && m.X == piece.Position.X)))
                    {
                        return true;
                    }
                }
            }
            if (!GameInstance.Pieces.Where(p => p.PieceColor != this.PieceColor)
                    .Any(p => p.AvailableMoves.Any(m => m.Y == y && m.X == x)))
            {
                return true;
            }

            return false;
        }
        catch (IndexOutOfRangeException e)
        {
            return false;
        }
    }
}