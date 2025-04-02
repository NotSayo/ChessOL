using Shared.Types;

namespace Shared.Chess.Pieces;

public class Pawn : APiece
{
    public sealed override List<Vector> Moves { get; set; } = new();

    public override bool Repetitive { get; set; } = false;
    public bool IsLastMoveDouble { get; private set; } = false;
    public bool IsFirstMove { get; private set; } = true;

    public Pawn(EPieceColor color)
    {
        PieceColor = color;
        if (PieceColor == EPieceColor.Black)
        {
            Moves.Add(new Vector(1, 0));
        }
        else
        {
            Moves.Add(new Vector(-1, 0));
        }
    }

    public override void CheckAvailableMoves()
    {
        foreach (var vec in Moves)
        {
            try
            {
                if (GameInstance.Board[Position.Y + vec.Y, Position.X + vec.X] is null)
                {
                    AvailableMoves.Add(new Vector() {Y = Position.Y + vec.Y, X = Position.X + vec.X});
                    if (!IsFirstMove)
                        break;
                    if(GameInstance.Board[Position.Y + vec.Y * 2, Position.X + vec.X] is null)
                        AvailableMoves.Add(new Vector() {Y = Position.Y + vec.Y * 2, X = Position.X + vec.X});
                }

                var sidePieces = new IPiece?[]
                {
                    GameInstance.Board[Position.Y + vec.Y, Position.X + vec.X + (-1)],
                    GameInstance.Board[Position.Y + vec.Y, Position.X + vec.X + 1]
                };
                foreach (var piece in sidePieces)
                {
                    if (piece is not null)
                    {
                        if(piece.PieceColor != this.PieceColor)
                            AvailableMoves.Add(new Vector() {Y = piece.Position.Y, X = piece.Position.X});
                    }
                }
            }
            catch (IndexOutOfRangeException e)
            {
            }
        }
    }

    public override void Move()
    {
        IsFirstMove = false;
        IsLastMoveDouble = false;
        if (Moves.Count > 2)
        {
            IsLastMoveDouble = true;
            Moves = new List<Vector>();
            if(PieceColor == EPieceColor.Black)
                Moves.Add(new Vector(1, 0));
            else
                Moves.Add(new Vector(-1, 0));
        }
        base.Move();

    }
}