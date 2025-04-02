using Shared.Types;

namespace Shared.Chess.Pieces;

public class Bishop : APiece
{
    public override List<Vector> Moves { get; set; } = new()
    {
        new Vector(-1, -1),
        new Vector(1, 1),
        new Vector(-1, 1),
        new Vector(1, -1)
    };

    public override bool Repetitive { get; set; } = true;
}