using System.Runtime.CompilerServices;
using Shared.Types;

namespace Shared.Chess.Pieces;

public class King : APiece
{
    public override List<Vector> Moves { get; set; } = new()
    {
        new Vector(1, 0),
        new Vector(1, 1),
        new Vector(0, 1),
        new Vector(-1, 1),
        new Vector(-1, 0),
        new Vector(-1, -1),
        new Vector(0, -1),
        new Vector(1, -1),
    };

    public override bool Repetitive { get; set; } = false;
    public bool IsInCheck { get; set; } = false;
    public bool IsInCheckmate { get; set; } = false;
    public bool IsInStalemate { get; set; } = false;
    private List<Vector> _checkingFields = new List<Vector>();

    public override void CheckAvailableMoves()
    {
        _checkingFields = new List<Vector>();
        AvailableMoves = new List<Vector>();
        GameInstance.Pieces.Where(p => p.GetType() != this.GetType()).ToList().ForEach(p => p.CheckAvailableMoves());
        if (GameInstance.Pieces.Where(p => p.PieceColor != this.PieceColor)
            .Any(p => p.AvailableMoves.Any(m => m.Y == this.Position.Y && m.X == this.Position.X)))
        {
            Console.WriteLine("King is in Check");
            IsInCheck = true;
            CheckAvailableMovesDuringCheck();
            return;
        }
        else
            IsInCheck = false;
        CheckKingMoves();
    }

    private void CheckKingMoves()
    {
        foreach (var vec in Moves)
        {
            if (CheckIfPositionIsAvailable(Position.Y + vec.Y, Position.X + vec.X))
            {
                AvailableMoves.Add(new Vector() {Y = Position.Y + vec.Y, X = Position.X + vec.X});
            }
        }
    }

    private void CheckAvailableMovesDuringCheck()
    {
        List<IPiece> checkingPieces = GameInstance.Pieces.Where(p => p.PieceColor != this.PieceColor)
            .Where(p => p.AvailableMoves.Any(m => m.Y == this.Position.Y && m.X == this.Position.X)).ToList();
        Console.WriteLine("Checking count: " + checkingPieces.Count);
        if (checkingPieces.Count >= 1)
        {
            foreach (var checkingPiece in checkingPieces)
            {
                // getting all squares between kind and attacking piece
                foreach (var vec in checkingPiece.Moves)
                {
                    List<Vector> currentMoves = new();
                    bool rightLine = false;
                    try
                    {
                        int increaseX = 0;
                        int increaseY = 0;
                        int changeX = vec.X > 0 ? 1 : -1;
                        int changeY = vec.Y > 0 ? 1 : -1;

                        do
                        {
                            if (GameInstance.Board[checkingPiece.Position.Y + vec.Y + increaseY,
                                    checkingPiece.Position.X + vec.X + increaseX] is null)
                            {
                                var newPosition = new Vector(checkingPiece.Position.Y + vec.Y + increaseY,
                                    checkingPiece.Position.X + vec.X + increaseX);
                                if (newPosition != Position)
                                    currentMoves.Add(newPosition);

                                if (vec.Y != 0)
                                    increaseY += changeY;
                                if (vec.X != 0)
                                    increaseX += changeX;
                                if (vec == new Vector(0, 0)) break;
                            }
                            else
                            {
                                if (GameInstance.Board[checkingPiece.Position.Y + vec.Y + increaseY,
                                        checkingPiece.Position.X + vec.X + increaseX]! == this)
                                {
                                    rightLine = true;
                                }

                                if (!rightLine)
                                {
                                    currentMoves = new List<Vector>();
                                    break;
                                }

                                var newPosition = new Vector(checkingPiece.Position.Y + vec.Y + increaseY,
                                    checkingPiece.Position.X + vec.X + increaseX);
                                currentMoves.Add(newPosition);

                                if (vec.Y != 0)
                                    increaseY += changeY;
                                if (vec.X != 0)
                                    increaseX += changeX;

                            }
                        } while (checkingPiece.Repetitive);
                    }
                    catch (IndexOutOfRangeException e)
                    {
                    }
                    finally
                    {
                        if (rightLine)
                        {
                            rightLine = false;
                            _checkingFields.AddRange(currentMoves);
                            _checkingFields = _checkingFields.Distinct().ToList();
                            currentMoves = new();
                        }
                    }
                }
            }

            // _checkingFields.ForEach(s => Console.WriteLine("Line: "+ s));
            var kingPieces = GameInstance.Pieces.Where(p => p.PieceColor == this.PieceColor && p != this).ToList();

            foreach (var piece in kingPieces)
            {
                var newMoves = new List<Vector>();
                foreach (var vec in _checkingFields)
                {
                    if (piece.AvailableMoves.Any(m => m.Y == vec.Y && m.X == vec.X))
                        newMoves.AddRange(piece.AvailableMoves.Where(m => m.Y == vec.Y && m.X == vec.X));
                }

                if (piece is Pawn p)
                {
                    var move = p.Moves.First(v => v.Y == 1 || v.Y == -1);
                    if(p.Position.Y + move.Y < 0 || p.Position.Y + move.Y > 7 ||
                       p.Position.X + move.X - 1 < 0 || p.Position.X + move.X - 1 > 7) {}
                    else if (GameInstance.Board[p.Position.Y + move.Y, p.Position.X + move.X - 1] is not null)
                    {
                        if (GameInstance.Board[p.Position.Y + move.Y, p.Position.X + move.X - 1]!.PieceColor != this.PieceColor)
                            newMoves.Add(new Vector() {Y = p.Position.Y + move.Y, X = p.Position.X + move.X - 1 });
                    }
                    if(p.Position.Y + move.Y < 0 || p.Position.Y + move.Y > 7 ||
                       p.Position.X + move.X + 1 < 0 || p.Position.X + move.X + 1 > 7) {}
                    else if (GameInstance.Board[p.Position.Y + move.Y, p.Position.X + move.X + 1] is not null)
                    {
                        if (GameInstance.Board[p.Position.Y + move.Y, p.Position.X + move.X + 1]!.PieceColor != this.PieceColor)
                            newMoves.Add(new Vector() {Y = p.Position.Y + move.Y, X = p.Position.X + move.X + 1 });
                    }
                }
                piece.AvailableMoves = newMoves;
            }

        }
        else
            GameInstance.Pieces.Where(p => p.PieceColor == this.PieceColor).ToList().ForEach(p => p.AvailableMoves = new ());


        foreach (var vec in Moves)
        {
            if (CheckIfPositionIsAvailable(Position.Y + vec.Y, Position.X + vec.X))
            {
                var newPosition = new Vector() { Y = Position.Y + vec.Y, X = Position.X + vec.X };
                if(!AvailableMoves.Contains(newPosition))
                    AvailableMoves.Add(newPosition);
            }
        }
    }

    private bool CheckIfPositionIsAvailable(int y, int x)
    {
        // Console.WriteLine(_checkingFields.Count);
        // _checkingFields.ForEach(s => Console.WriteLine(s));
        try
        {
            var otherKingFields = GameInstance.Pieces.OfType<King>().First(p => p.PieceColor != this.PieceColor)
                .GetTheoreticalMoves();
            if (_checkingFields.Any(f => f.X == x && f.Y == y))
            {
                var field = _checkingFields.First(f => f.X == x && f.Y == y);
                if (GameInstance.Board[field.Y, field.X] is null)
                    return false;

            }
            if(otherKingFields.Any(f => f.X == x && f.Y == y))
                return false;

            if (GameInstance.Board[y, x] is not null)
            {
                var piece = GameInstance.Board[y, x];
                if (piece!.PieceColor != this.PieceColor)
                {
                    if (!GameInstance.Pieces.Where(p => p.PieceColor != this.PieceColor)
                            .Any(p => p.VisibleFields.Any(m => m == piece.Position)))
                    {
                        return true;
                    }
                }

                return false;
            }
            if (!IsSquareUnderAttack(y,x, this.PieceColor == EPieceColor.Black ? EPieceColor.White : EPieceColor.Black))
                return true;

            return false;
        }
        catch (IndexOutOfRangeException e)
        {
            return false;
        }
    }

    public bool IsSquareUnderAttack(int y, int x, EPieceColor byColor)
    {
        return GameInstance.Pieces
            .Where(p => p.PieceColor == byColor)
            .SelectMany(p => p.VisibleFields)
            .Any(f => f.Y == y && f.X == x);
    }

    public List<Vector> GetTheoreticalMoves()
    {
        var theoreticalMoves = new List<Vector>();
        foreach (var vec in Moves)
        {
            theoreticalMoves.Add(new Vector(Position.Y + vec.Y, Position.X + vec.X));
        }
        theoreticalMoves.Add(new (Position.Y, Position.X));
        return theoreticalMoves;
    }

}

