using Shared.Types;

namespace Shared.Chess.Pieces;

public class Knight : APiece
{
    public override List<Vector> Moves { get; set; } = new()
    {
        new (-2, 1),
        new (-2, -1),
        new (-1,2),
        new (1,2),
        new (2,1),
        new (2,-1),
        new (-1, -2),
        new (1, -2),
    };

    public override bool Repetitive { get; set; } = false;
}