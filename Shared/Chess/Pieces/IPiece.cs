using System.Drawing;
using Shared.Chess.GameManager;
using Shared.Types;
using Color = MudBlazor.Color;

namespace Shared.Chess.Pieces;

public interface IPiece
{
    public GameInstance GameInstance { get; set; }
    public string PieceColor { get; set; }
    public Vector Position { get; set; }
    public List<Vector> Moves { get; set; }
    public bool Repetitive { get; set; }
    public List<Vector> AvailableMoves { get; set; }
    public string Icon { get; set; }
    public string Identifier { get; set; }

    public void CheckAvailableMoves();
    public void Move();
}