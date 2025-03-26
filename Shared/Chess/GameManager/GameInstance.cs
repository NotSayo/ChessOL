using Shared.Chess.Pieces;

namespace Shared.Chess.GameManager;

public class GameInstance
{
    public List<IPiece> Pieces { get; set; }
    public IPiece?[,] Board = new IPiece[8,8]; // make readonly later


}