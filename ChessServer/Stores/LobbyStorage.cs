using Shared.Lobby;

namespace ChessServer.Stores;

public static class LobbyStorage
{
        public static List<LobbyInstance> Lobbies { get; set; } = new List<LobbyInstance>();
}