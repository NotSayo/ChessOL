using Shared.Chess.GameManager;
using Shared.Types;

namespace Shared.Chess.Pieces;

public class Rook : APiece
{
    public override List<Vector> Moves { get; set; } = new List<Vector>()
    {
        new (1,0),
        new (-1,0),
        new (0,1),
        new (0,-1)
    };

    public override bool Repetetive { get; set; } = true;
}