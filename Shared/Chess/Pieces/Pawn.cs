using Shared.Types;

namespace Shared.Chess.Pieces;

public class Pawn : APiece
{
    public sealed override List<Vector> Moves { get; set; } = new();

    public override bool Repetitive { get; set; } = false;
    public bool IsLastMoveDouble { get; set; } = false;
    public bool IsFirstMove { get; private set; } = true;

    public Pawn(EPieceColor color)
    {
        PieceColor = color;
        if (PieceColor == EPieceColor.Black)
        {
            Moves.Add(new Vector(1,0));
        }
        else
        {
            Moves.Add(new Vector(-1,0));
        }
    }

    public override void CheckAvailableMoves()
    {
        AvailableMoves = new List<Vector>();
        var king = GameInstance.Pieces.OfType<King>().First(e => e.PieceColor == this.PieceColor);
        if (king.IsInCheck)
        {
            return;
        }
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
                
                if(Position.Y + vec.Y < 0 || Position.Y + vec.Y > 7 ||
                   Position.X + vec.X - 1 < 0 || Position.X + vec.X - 1 > 7) {}
                else if (GameInstance.Board[Position.Y + vec.Y, Position.X + vec.X - 1] is not null)
                {
                    if (GameInstance.Board[Position.Y + vec.Y, Position.X + vec.X - 1]!.PieceColor != this.PieceColor)
                        AvailableMoves.Add(new Vector() {Y = Position.Y + vec.Y, X = Position.X + vec.X - 1 });
                }
                if(Position.Y + vec.Y < 0 || Position.Y + vec.Y > 7 ||
                   Position.X + vec.X + 1 < 0 || Position.X + vec.X + 1 > 7) {}
                else if (GameInstance.Board[Position.Y + vec.Y, Position.X + vec.X + 1] is not null)
                {
                    if (GameInstance.Board[Position.Y + vec.Y, Position.X + vec.X + 1]!.PieceColor != this.PieceColor)
                        AvailableMoves.Add(new Vector() {Y = Position.Y + vec.Y, X = Position.X + vec.X + 1 });
                }
            }
            catch (IndexOutOfRangeException e)
            {
            }
            if(Position.Y < 0 || Position.Y > 7 ||
               Position.X + 1 < 0 || Position.X + 1 > 7) {}
            else if (GameInstance.Board[Position.Y, Position.X + vec.X + 1] is Pawn pr)
            {
                if (pr.PieceColor != this.PieceColor && pr.IsLastMoveDouble)
                {
                    AvailableMoves.Add(new Vector() { Y = Position.Y + vec.Y, X = Position.X + vec.X + 1 });
                }
            }
            if(Position.Y < 0 || Position.Y > 7 ||
               Position.X - 1 < 0 || Position.X - 1 > 7) {}
            else if (GameInstance.Board[Position.Y, Position.X + vec.X - 1] is Pawn pl)
            {
                if (pl.PieceColor != this.PieceColor && pl.IsLastMoveDouble)
                {
                    AvailableMoves.Add(new Vector() { Y = Position.Y + vec.Y, X = Position.X + vec.X - 1 });
                }
            }
        }

        try
        {
            var sidePieces = new IPiece?[]
            {
                GameInstance.Board[Position.Y, Position.X + (-1)],
                GameInstance.Board[Position.Y, Position.X + 1]
            };
            foreach (var piece in sidePieces)
            {
                if (piece is not null)
                {
                    if (piece.PieceColor != this.PieceColor && piece is Pawn p)
                    {
                        if (IsLastMoveDouble)
                            AvailableMoves.Add(new Vector() { Y = piece.Position.Y + Moves.First(m => m.Y == -1 || m.Y == 1).Y, X = piece.Position.X });
                    }
                }
            }
        }
        catch (IndexOutOfRangeException)
        {

        }

        AvailableMoves = AvailableMoves.Distinct().ToList();
    }

    public override void Move() // TODO!!!
    {
        IsFirstMove = false;
        IsLastMoveDouble = false;
        if (Moves.Count > 2)
        {
            IsLastMoveDouble = true;
            Moves = new List<Vector>();
            if (PieceColor == EPieceColor.Black)
                Moves.Add(new Vector(1, 0));
            else
                Moves.Add(new Vector(-1, 0));
        }
        base.Move();

    }
}