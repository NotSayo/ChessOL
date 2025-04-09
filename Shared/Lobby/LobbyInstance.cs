using MudBlazor;

namespace Shared.Lobby;

public class LobbyInstance
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string OwnerId { get; set; }
    public required string OwnerName { get; set; }
    public List<string> Players { get; set; } = new List<string>();

    public required bool IsPrivate { get; set; }
    public string Passcode { get; set; } = string.Empty;
}