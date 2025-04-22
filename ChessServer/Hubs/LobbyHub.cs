using System.Security.Claims;
using ChessServer.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Shared.Lobby;

namespace ChessServer.Hubs;

[Authorize]
public class LobbyHub : Hub
{
    public async Task GetStatus()
    {
        await Clients.Caller.SendAsync("GetStatus", "Running");
        await Clients.Caller.SendAsync("GetStatus", Context.User.Claims.First(s => s.Type == ClaimTypes.NameIdentifier).Value);
        await Clients.Caller.SendAsync("GetStatus", Context.User.Claims.First(s => s.Type == ClaimTypes.Name).Value);
    }

    public async Task GetLobby()
    {
        await Clients.Caller.SendAsync("ReceiveLobby", LobbyStorage.Lobbies.Select(l => new LobbyInfo
        {
            Id = l.Id,
            Name = l.Name,
            OwnerName = l.OwnerName,
            Players = l.Players
        }));
    }

    public async Task SendLobbyUpdate()
    {;
        await Clients.All.SendAsync("ReceiveLobby", LobbyStorage.Lobbies.Select(l => new LobbyInfo
        {
            Id = l.Id,
            Name = l.Name,
            OwnerName = l.OwnerName,
            Players = l.Players
        }));
    }

    public async Task AddLobby(string name, bool isPrivate, string passcode = "")
    {
        LobbyStorage.Lobbies.Add(new ()
        {
            Id = LobbyStorage.Lobbies.Count + 1 + "",
            Name = name,
            OwnerId = Context.User!.Claims.First(u => u.Type == ClaimTypes.NameIdentifier).Value,
            OwnerName = Context.User!.Claims.First(u => u.Type == ClaimTypes.Name).Value,
            Players = [Context.User!.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value],
            IsPrivate = false,
            Passcode = passcode
        });
        await SendLobbyUpdate();
    }


}