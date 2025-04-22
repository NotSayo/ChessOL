using Shared.Chess.GameManager;
using Shared.Types;

namespace Shared.Chess.Pieces;

public interface IPiece
{
    public GameInstance GameInstance { get; set; }
    public EPieceColor PieceColor { get; set; }
    public Vector Position { get; set; }
    public string Icon { get; set; }
    public List<Vector> Moves { get; set; }
    public bool Repetitive { get; set; }
    public List<Vector> AvailableMoves { get; set; }
    public List<Vector> VisibleFields { get; set; }
    public bool Active { get; set; }

    public void CheckAvailableMoves();
    public void CheckVisibleFields();
    public void Move(Vector location);
}