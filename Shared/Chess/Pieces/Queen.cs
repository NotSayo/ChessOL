using Shared.Types;

namespace Shared.Chess.Pieces;

public class Queen : APiece
{
    public override List<Vector> Moves { get; set; } = new()
    {
        new (1,0),
        new (1,1),
        new (0,1),
        new (-1,1),
        new (-1,0),
        new (-1,-1),
        new (0,-1),
        new (1,-1),
    };
    public override bool Repetitive { get; set; } = true;
}