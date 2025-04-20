using MudBlazor;
using Shared.Chess.GameManager;
using Shared.Types;

namespace Shared.Chess.Pieces;

public abstract class APiece : IPiece
{
    public GameInstance GameInstance { get; set; }
    public EPieceColor PieceColor { get; set; }
    public string Icon { get; set; }
    public Vector Position { get; set; }
    public abstract List<Vector> Moves { get; set; }
    public abstract bool Repetitive { get; set; }
    public List<Vector> AvailableMoves { get; set; } = new();
    public List<Vector> VisibleFields { get; set; } = new();
    public bool Active { get; set; } = true;
    public virtual void CheckAvailableMoves()
    {
        if(!Active)
            return;
        CheckVisibleFields();
        var king = GameInstance.Pieces.OfType<King>().FirstOrDefault(e => e.PieceColor == this.PieceColor);
        if (king is not null)
        {
            if (king.IsInCheck)
            {
                return;
            }
        }
        AvailableMoves = new List<Vector>();

        foreach (var vec in Moves)
        {
            try
            {
                int increaseX = 0;
                int increaseY = 0;
                int changeX = vec.X > 0 ? 1 : -1;
                int changeY = vec.Y > 0 ? 1 : -1;
                do
                {
                    // Console.WriteLine((Position.Y + vec.Y + increaseY) + " " + (Position.X + vec.X + increaseX));
                    if(GameInstance.Board[Position.Y + vec.Y + increaseY, Position.X + vec.X + increaseX] is null)
                    {
                        var newPosition = new Vector(Position.Y + vec.Y + increaseY, Position.X + vec.X + increaseX);
                        if(newPosition != Position)
                            AvailableMoves.Add(newPosition);

                        if (vec.Y != 0)
                            increaseY += changeY;
                        if (vec.X != 0)
                            increaseX += changeX;
                        if (vec == new Vector(0, 0)) break;
                    }
                    else
                    {
                        if (GameInstance.Board[Position.Y + vec.Y + increaseY, Position.X + vec.X + increaseX]!.PieceColor != PieceColor)
                        {
                            AvailableMoves.Add(new Vector(Position.Y + vec.Y + increaseY, Position.X + vec.X + increaseX));
                        }
                        break;
                    }
                } while (Repetitive);
            }
            catch (IndexOutOfRangeException e)
            {
            }
        }
    }

    public virtual void CheckVisibleFields()
    {
        if(!Active)
            return;
        var king = GameInstance.Pieces.OfType<King>().FirstOrDefault(e => e.PieceColor == this.PieceColor);
        if (king is not null)
        {
            if (king.IsInCheck)
            {
                return;
            }
        }
        VisibleFields = new List<Vector>();

        foreach (var vec in Moves)
        {
            try
            {
                int increaseX = 0;
                int increaseY = 0;
                int changeX = vec.X > 0 ? 1 : -1;
                int changeY = vec.Y > 0 ? 1 : -1;
                do
                {
                    if(GameInstance.Board[Position.Y + vec.Y + increaseY, Position.X + vec.X + increaseX] is null)
                    {
                        var newPosition = new Vector(Position.Y + vec.Y + increaseY, Position.X + vec.X + increaseX);
                        if(newPosition != Position)
                            VisibleFields.Add(newPosition);

                        if (vec.Y != 0)
                            increaseY += changeY;
                        if (vec.X != 0)
                            increaseX += changeX;
                        if (vec == new Vector(0, 0)) break;
                    }
                    else
                    {
                        VisibleFields.Add(new Vector(Position.Y + vec.Y + increaseY, Position.X + vec.X + increaseX));
                        break;
                    }
                } while (Repetitive);
            }
            catch (IndexOutOfRangeException e)
            {
            }
        }
    }

    public virtual void Move(Vector location)
    {
        if (!Active)
            return;
        if (GameInstance.CurrentTurn != PieceColor)
            return;
        if (!AvailableMoves.Contains(location))
            return;
        GameInstance.RemoveFromBoard(this);
        this.Position = location;
        GameInstance.AddToBoard(this);
    }
}