using Shared.Chess.Pieces;

namespace Shared.Chess.GameManager;

public class GameInstance
{
    public List<IPiece> Pieces { get; set; }
    public readonly IPiece?[,] Board = new IPiece[8,8];



}