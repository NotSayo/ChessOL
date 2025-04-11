using System.Security.Claims;
using ChessServer.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Storage;

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
        if(QueueStore.ActiveQueue.Contains(Context.UserIdentifier!))
        {
            await Clients.Caller.SendAsync("QueueStatus", "You are already in the queue.");
            return;
        }
        QueueStore.ActiveQueue.Add(Context.UserIdentifier!);
        await Clients.Caller.SendAsync("QueueStatus", "You have entered the queue.");
        var position = QueueStore.ActiveQueue.IndexOf(Context.UserIdentifier!);
        await Clients.Caller.SendAsync("QueuePosition",  position == -1 ? 0 : position + 1);
        await Clients.All.SendAsync("QueueAmount", QueueStore.ActiveQueue.Count);
        await CheckIfInQueue();
        await EvaluateQueue();
    }

    public async Task GetQueueStatus()
    {
        var position = QueueStore.ActiveQueue.IndexOf(Context.UserIdentifier!);
        await Clients.Caller.SendAsync("QueuePosition",  position == -1 ? 0 : position + 1);
        await Clients.Caller.SendAsync("QueueAmount", QueueStore.ActiveQueue.Count);
    }

    private async Task EvaluateQueue()
    {
        if (QueueStore.ActiveQueue.Count >= 3)
        {
            var player1 = QueueStore.ActiveQueue.First();
            QueueStore.ActiveQueue.RemoveAt(0);
            var player2 = QueueStore.ActiveQueue.First();
            QueueStore.ActiveQueue.RemoveAt(0);
            await Clients.User(player1).SendAsync("GameStart", player2);
            await Clients.User(player2).SendAsync("GameStart", player1);
            await SendRequireUpdate();
        }
    }

    private async Task SendRequireUpdate()
    {
        await Clients.All.SendAsync("RequireUpdate");
    }

    public async Task CheckIfInQueue()
    {
        await Clients.Caller.SendAsync("QueueCheck", QueueStore.ActiveQueue.Contains(Context.UserIdentifier!));
    }

    public async Task LeaveQueue()
    {
        var result = QueueStore.ActiveQueue.Remove(Context.UserIdentifier!);
        if(!result)
            Console.WriteLine("User not in queue");
        var position = QueueStore.ActiveQueue.IndexOf(Context.UserIdentifier!);
        await Clients.Caller.SendAsync("QueuePosition",  position == -1 ? 0 : position + 1);
        await Clients.All.SendAsync("QueueAmount", QueueStore.ActiveQueue.Count);
        await CheckIfInQueue();
    }
}