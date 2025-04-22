using Shared.Types;

namespace ChessServer.Stores;

public static class QueueStore
{
    public static List<Player> ActiveQueue { get; set; } = new();
}