using System.Numerics;
using System.Security.Claims;
using ChessServer.Stores;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Shared.Chess.GameManager;
using Shared.Chess.Pieces;
using Shared.Types;
using Vector = Shared.Types.Vector;

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

    public async Task MovePiece(string gamecode, Vector oldPosition, Vector newPosition)
    {
        if (!CheckAccess(gamecode))
        {
            await Clients.Caller.SendAsync("NoAccess");
            return;
        }

        var game = GameStore.ActiveGame.FirstOrDefault(s => s.GameCode == gamecode);
        var username = Context.User!.Claims.First(s => s.Type == ClaimTypes.Name).Value;

        var piece = game!.Instance.Pieces.FirstOrDefault(p => p.Position == oldPosition);
        var playerColor = game.Player1.PlayerId == Context.UserIdentifier ? EPieceColor.White : EPieceColor.Black;
        if (!CheckPieceMove(piece, oldPosition, newPosition, playerColor, game.Instance))
        {
            await Clients.Caller.SendAsync("InvalidMove");
            return;
        }

        piece!.GameInstance.MovePiece(piece, newPosition);
        game.UpdateGameInfo();

        await Clients.Users(game.Player1.PlayerId, game.Player2.PlayerId).SendAsync("GameInfoUpdate", game.GameInfo);
        Console.WriteLine("Game updated: " + game.GameCode);
    }

    private bool CheckAccess(string gameCode)
    {
        var game = GameStore.ActiveGame.FirstOrDefault(s => s.GameCode == gameCode);
        if (game is null)
            return false;

        var username = Context.User!.Claims.First(s => s.Type == ClaimTypes.Name).Value;
        if (game.Player1.Name != username && game.Player2.Name != username)
            return false;
        return true;
    }

    private bool CheckPieceMove(IPiece? piece, Vector oldPosition, Vector newPosition, EPieceColor playerColor, GameInstance instance)
    {
        if(piece is null)
            return false;

        if (instance.Board[newPosition.Y, newPosition.X] is not null)
        {
            if(instance.Board[newPosition.Y, newPosition.X]!.PieceColor == playerColor)
                return false;
        }

        if (piece.PieceColor != instance.CurrentTurn)
            return false;

        if (piece.PieceColor != playerColor)
            return false;

        if (piece.Position != oldPosition)
            return false;

        if (!piece.Active)
            return false;

        if(piece.AvailableMoves.All(m => m != newPosition))
            return false;


        return true;
    }
}