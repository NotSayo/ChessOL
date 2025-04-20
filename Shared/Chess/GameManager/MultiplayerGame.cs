using System.Text.Json.Serialization;
using Shared.Chess.Pieces;
using Shared.Chess.SimplePieces;
using Shared.Types;

namespace Shared.Chess.GameManager;

public class MultiplayerGame
{
    public GameInfo GameInfo { get; set; }

    public required Player Player1 { get; set; }
    public required Player Player2 { get; set; }
    public string GameCode { get; set; }
    [JsonIgnore]
    public GameInstance? Instance { get; set; }

    public MultiplayerGame()
    {
    }

    public MultiplayerGame(GameInstance instance)
    {
        Instance = instance;
        UpdateGameInfo();
    }

    public void UpdateGameInfo()
    {
        var simplePieces = new List<SimplePiece>();
        foreach (var piece in Instance.Pieces)
        {
            var sPiece = new SimplePiece()
            {
                EPieceColor = piece.PieceColor,
                Position = new Vector(piece.Position.Y, piece.Position.X),
                EPieceType = piece switch
                {
                    Rook => EPieceType.Rook,
                    Knight => EPieceType.Knight,
                    Bishop => EPieceType.Bishop,
                    Queen => EPieceType.Queen,
                    King => EPieceType.King,
                    Pawn => EPieceType.Pawn,
                    _ => throw new ArgumentOutOfRangeException()
                },
                AvailableMoves = piece.AvailableMoves
            };
            sPiece.AssignIcon();
            simplePieces.Add(sPiece);
        }

        GameInfo = new GameInfo
        {
            CurrentTurn = Instance.CurrentTurn,
            Pieces = simplePieces,
        };
    }
}