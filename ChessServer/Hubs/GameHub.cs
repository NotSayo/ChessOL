using System.Security.Claims;
using ChessServer.Stores;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChessServer.Hubs;

[Authorize(JwtBearerDefaults.AuthenticationScheme)]
public class GameHub : Hub
{
    public async Task GetGameInfo(string gameCode)
    {
        var gameInstance = GameStore.ActiveGame.FirstOrDefault(g => g.GameCode == gameCode);
        if (gameInstance is null)
        {
            Console.WriteLine("No access for user: " + Context.User!.Claims.First(c => c.Type == ClaimTypes.Name).Value);
            await Clients.Caller.SendAsync("NoAccess");
            return;
        }

        var username = Context.User!.Claims.First(s => s.Type == ClaimTypes.Name).Value;
        if (gameInstance.Player1.Name != username && gameInstance.Player2.Name != username)
        {
            await Clients.Caller.SendAsync("NoAccess");
            Console.WriteLine("No access for user: " + username);
            return;
        }

        Console.WriteLine("Game Info: " + gameInstance.GameCode);
        Console.WriteLine("P1: " + gameInstance.Player1.Name);
        Console.WriteLine("P2: " + gameInstance.Player2.Name);
        await Clients.Caller.SendAsync("GameInfo",  gameInstance!);
        await Clients.Caller.SendAsync("PlayerName", Context.User!.Claims.First(c => c.Type == ClaimTypes.Name).Value);
    }
}