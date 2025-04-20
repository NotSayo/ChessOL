using System.Security.Claims;
using ChessServer.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Storage;
using Shared.Chess.GameManager;

namespace ChessServer.Hubs;

[Authorize]
public class QueueHub : Hub
{
    public async Task GetStatus()
    {
        await Clients.Caller.SendAsync("GetStatus", "Running");
    }

    public async Task EnterQueue()
    {
        if(QueueStore.ActiveQueue.Any(p => p.PlayerId == Context.UserIdentifier))
        {
            await Clients.Caller.SendAsync("QueueStatus", "You are already in the queue.");
            return;
        }
        QueueStore.ActiveQueue.Add(new ()
        {
            PlayerId = Context.UserIdentifier!,
            Name = Context.User!.Claims.First(s => s.Type == ClaimTypes.Name).Value
        });
        await Clients.Caller.SendAsync("QueueStatus", "You have entered the queue.");
        var position = QueueStore.ActiveQueue.IndexOf(QueueStore.ActiveQueue.First(p => p.PlayerId == Context.UserIdentifier!));
        await Clients.Caller.SendAsync("QueuePosition",  position == -1 ? 0 : position + 1);
        await Clients.All.SendAsync("QueueAmount", QueueStore.ActiveQueue.Count);
        await CheckIfInQueue();
        await EvaluateQueue();
    }

    public async Task GetQueueStatus()
    {
        var position = QueueStore.ActiveQueue.IndexOf(QueueStore.ActiveQueue.FirstOrDefault(p => p.PlayerId == Context.UserIdentifier!));
        await Clients.Caller.SendAsync("QueuePosition",  position == -1 ? 0 : position + 1);
        await Clients.Caller.SendAsync("QueueAmount", QueueStore.ActiveQueue.Count);
    }

    private async Task EvaluateQueue()
    {
        if (QueueStore.ActiveQueue.Count >= 2)
        {
            var player1 = QueueStore.ActiveQueue.First();
            QueueStore.ActiveQueue.RemoveAt(0);
            var player2 = QueueStore.ActiveQueue.First();
            QueueStore.ActiveQueue.RemoveAt(0);

            var randomFirstPlayer = new Random().NextDouble() > 0.5 ? player1 : player2;
            var randomSecondPlayer = randomFirstPlayer == player1 ? player2 : player1;

            var instance = new GameInstance();
            instance.InitializeGame();
            string gameCode = Guid.NewGuid().ToString()[..10];
            GameStore.ActiveGame.Add(new(instance)
            {
                Player1 = randomFirstPlayer,
                Player2 = randomSecondPlayer,
                GameCode = gameCode,
                Instance = instance
            });
            await Clients.User(player1.PlayerId).SendAsync("GameStart", gameCode);
            await Clients.User(player1.PlayerId).SendAsync("PlayerName", player1.Name);

            await Clients.User(player2.PlayerId).SendAsync("GameStart", gameCode);
            await Clients.User(player2.PlayerId).SendAsync("PlayerName", player2.Name);
            await SendRequireUpdate();
        }
    }

    private async Task SendRequireUpdate()
    {
        await Clients.All.SendAsync("RequireUpdate");
    }

    public async Task CheckIfInQueue()
    {
        await Clients.Caller.SendAsync("QueueCheck", QueueStore.ActiveQueue.Select(p => p.PlayerId).Contains(Context.UserIdentifier!));
    }

    public async Task LeaveQueue()
    {
        var result = QueueStore.ActiveQueue.Remove(QueueStore.ActiveQueue.First(p => p.PlayerId == Context.UserIdentifier!));
        if(!result)
            Console.WriteLine("User not in queue");
        await Clients.Caller.SendAsync("QueuePosition",  0);
        await Clients.All.SendAsync("QueueAmount", QueueStore.ActiveQueue.Count);
        await CheckIfInQueue();
        await SendRequireUpdate();
    }
}