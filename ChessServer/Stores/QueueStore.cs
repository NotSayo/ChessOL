namespace ChessServer.Stores;

public static class QueueStore
{
    public static List<string> ActiveQueue { get; set; } = new();
}