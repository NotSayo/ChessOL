namespace Shared.Lobby;

public class LobbyInfo
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string OwnerName { get; set; }
    public List<string> Players { get; set; } = new List<string>();
}