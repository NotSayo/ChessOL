using Shared.Chess.GameManager;
using Shared.Types;
using Color = MudBlazor.Color;

namespace Shared.Chess.Pieces;

public abstract class APiece : IPiece
{
    public required GameInstance GameInstance { get; set; }
    public required string PieceColor { get; set; }
    public required Vector Position { get; set; }
    public abstract List<Vector> Moves { get; set; }
    public abstract bool Repetitive { get; set; }
    public List<Vector> AvailableMoves { get; set; } = new();
    public string Icon { get; set; }
    public string Identifier { get; set; }

    public virtual void CheckAvailableMoves()
    {
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
                        if (GameInstance.Board[Position.Y + vec.Y + increaseY, Position.X + vec.X + increaseX]!.PieceColor.ToString() != PieceColor)
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

    public virtual void Move()
    {
        throw new NotImplementedException();
    }
}