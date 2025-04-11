namespace Shared.Chess.GameManager;

public class MultiplayerGame
{
    public GameInstance Game = new GameInstance();
    public required string Player1;
    public required string Player2;
    public string GameCode;

    public MultiplayerGame()
    {
        GameCode = Guid.NewGuid().ToString()[..10];
    }
}