using System.Diagnostics;
using Shared.Chess.GameManager;
using Shared.Chess.Pieces;
using Shared.Types;

namespace TestChess;

public class Tests
{
    public GameInstance GameInstance { get; set; } = new GameInstance();

    [SetUp]
    public void Setup()
    {
        List<IPiece> pieces = new List<IPiece>()
        {
            // Black pieces
            new Rook() {GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector() {Y = 0, X = 0}},
            new Knight() {GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector() {Y = 0, X = 1}},
            new Bishop() {GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector() {Y = 0, X = 2}},
            new Queen() {GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector() {Y = 0, X = 3}},
            new King() {GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector() {Y = 0, X = 4}},
            new Bishop() {GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector() {Y = 0, X = 5}},
            new Knight() {GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector() {Y = 0, X = 6}},
            new Rook() {GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector() {Y = 0, X = 7}},

            // Black pawns
            new Pawn(EPieceColor.Black) {GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector() {Y = 1, X = 0}},
            new Pawn(EPieceColor.Black) {GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector() {Y = 1, X = 1}},
            new Pawn(EPieceColor.Black) {GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector() {Y = 1, X = 2}},
            new Pawn(EPieceColor.Black) {GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector() {Y = 1, X = 3}},
            new Pawn(EPieceColor.Black) {GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector() {Y = 1, X = 4}},
            new Pawn(EPieceColor.Black) {GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector() {Y = 1, X = 5}},
            new Pawn(EPieceColor.Black) {GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector() {Y = 1, X = 6}},
            new Pawn(EPieceColor.Black) {GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector() {Y = 1, X = 7}},

            // White pawns
            new Pawn(EPieceColor.White) {GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector() {Y = 6, X = 0}},
            new Pawn(EPieceColor.White) {GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector() {Y = 6, X = 1}},
            new Pawn(EPieceColor.White) {GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector() {Y = 6, X = 2}},
            new Pawn(EPieceColor.White) {GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector() {Y = 6, X = 3}},
            new Pawn(EPieceColor.White) {GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector() {Y = 6, X = 4}},
            new Pawn(EPieceColor.White) {GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector() {Y = 6, X = 5}},
            new Pawn(EPieceColor.White) {GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector() {Y = 6, X = 6}},
            new Pawn(EPieceColor.White) {GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector() {Y = 6, X = 7}},

            // White pieces
            new Rook() {GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector() {Y = 7, X = 0}},
            new Knight() {GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector() {Y = 7, X = 1}},
            new Bishop() {GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector() {Y = 7, X = 2}},
            new Queen() {GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector() {Y = 7, X = 3}},
            new King() {GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector() {Y = 7, X = 4}},
            new Bishop() {GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector() {Y = 7, X = 5}},
            new Knight() {GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector() {Y = 7, X = 6}},
            new Rook() {GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector() {Y = 7, X = 7}},
        };
        GameInstance.Pieces = pieces;

        foreach (var piece in pieces)
            GameInstance.Board[piece.Position.Y, piece.Position.X] = piece;
    }

    [Test]
    public void GetTimeForAllMoves()
    {
        int count = 0;
        Stopwatch sw = new Stopwatch();
        sw.Start();
        foreach (var piece in GameInstance.Board)
        {
            if (piece is null)
                continue;
            piece.CheckAvailableMoves();
            count++;
        }
        sw.Stop();
        Console.WriteLine(sw.ElapsedMilliseconds + "ms");
        Console.WriteLine("Count: " + count);
        Assert.Pass();
    }

    [Test]
    public void DisplayPiecesTest()
    {
        DisplayPieces();
        Assert.Pass();
    }


    public void DisplayPieces()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var piece = GameInstance.Board[i, j];
                if (piece is not null)
                {
                    Console.Write(GetPieceSymbol(piece));
                }
                else
                {
                    Console.Write("-");
                }
            }
            Console.WriteLine();
        }

        Assert.That(GameInstance.Board, Is.Not.Null);
    }

    private char GetPieceSymbol(IPiece piece)
    {
        return (piece.PieceColor, piece) switch
        {
            (EPieceColor.Black, Pawn) => '♙',
            (EPieceColor.Black, Rook) => '♖',
            (EPieceColor.Black, Knight) => '♘',
            (EPieceColor.Black, Bishop) => '♗',
            (EPieceColor.Black, Queen) => '♕',
            (EPieceColor.Black, King) => '♔',
            (EPieceColor.White, Pawn) => '♟',
            (EPieceColor.White, Rook) => '♜',
            (EPieceColor.White, Knight) => '♞',
            (EPieceColor.White, Bishop) => '♝',
            (EPieceColor.White, Queen) => '♛',
            (EPieceColor.White, King) => '♚',
            _ => '?'
        };
    }

    [Test]
    public void CheckAvailableMovesStart()
    {
        DisplayPieces();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var piece = GameInstance.Board[i, j];
                if (piece is null) continue;

                switch (piece)
                {
                    case Pawn p:
                        p.CheckAvailableMoves();
                        Console.WriteLine("Pawn: ");
                        p.AvailableMoves.ForEach(s => Console.WriteLine(s.ToString()));
                        Assert.That(p.AvailableMoves.Count, Is.EqualTo(2));
                        break;
                    case Bishop b:
                        b.CheckAvailableMoves();
                        Console.WriteLine("Bishop: ");
                        b.AvailableMoves.ForEach(s => Console.WriteLine(s.ToString()));
                        Assert.That(b.AvailableMoves.Count, Is.EqualTo(0));
                        break;
                    case Knight k:
                        k.CheckAvailableMoves();
                        Console.WriteLine("Knight: ");
                        k.AvailableMoves.ForEach(s => Console.WriteLine(s.ToString()));
                        Assert.That(k.AvailableMoves.Count, Is.EqualTo(2));
                        break;
                    case Rook r:
                        r.CheckAvailableMoves();
                        Console.WriteLine("Rook: ");
                        r.AvailableMoves.ForEach(s => Console.WriteLine(s.ToString()));
                        Assert.That(r.AvailableMoves.Count, Is.EqualTo(0));
                        break;
                    case Queen q:
                        q.CheckAvailableMoves();
                        Console.WriteLine("Quenn: ");
                        q.AvailableMoves.ForEach(s => Console.WriteLine(s.ToString()));
                        Assert.That(q.AvailableMoves.Count, Is.EqualTo(0));
                        break;
                    case King k:
                        k.CheckAvailableMoves();
                        Console.WriteLine("King: ");
                        k.AvailableMoves.ForEach(s => Console.WriteLine(s.ToString()));
                        Assert.That(k.AvailableMoves.Count, Is.EqualTo(0));
                        break;
                }
            }
        }
    }
    [Test]
    public void PawnAvailableMoves()
    {
        GameInstance.Board = new IPiece[8,8];
        var whitePawn = new Pawn(EPieceColor.White) { GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector { Y = 6, X = 3 } };
        GameInstance.Board[whitePawn.Position.Y, whitePawn.Position.X] = whitePawn;
        var blackPawn =new Pawn(EPieceColor.Black) { GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector { Y = 3, X = 6 } };
        GameInstance.Board[blackPawn.Position.Y, blackPawn.Position.X] = blackPawn;
        DisplayPieces();

        whitePawn.CheckAvailableMoves();
        blackPawn.CheckAvailableMoves();

        whitePawn.AvailableMoves.ForEach(s => Console.WriteLine(s.ToString()));
        blackPawn.AvailableMoves.ForEach(s => Console.WriteLine(s.ToString()));

        Assert.That(whitePawn.AvailableMoves.Count, Is.EqualTo(2));
        Assert.That(whitePawn.AvailableMoves, Does.Contain(new Vector { Y = 5, X = 3 }));
        Assert.That(whitePawn.AvailableMoves, Does.Contain(new Vector { Y = 4, X = 3 }));
        Assert.That(blackPawn.AvailableMoves.Count, Is.EqualTo(2));
        Assert.That(blackPawn.AvailableMoves, Does.Contain(new Vector { Y = 4, X = 6 }));
        Assert.That(blackPawn.AvailableMoves, Does.Contain(new Vector { Y = 5, X = 6 }));
    }

    [Test]
    public void KnightAvailableMoves()
    {
        GameInstance.Pieces = new List<IPiece>();
        GameInstance.Board = new IPiece[8, 8];
        var whiteKnight = new Knight { GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector { Y = 4, X = 4 } };
        GameInstance.Board[whiteKnight.Position.Y, whiteKnight.Position.X] = whiteKnight;
        GameInstance.Pieces.Add(whiteKnight);
        whiteKnight.AvailableMoves.ForEach(s => Console.WriteLine(s));

        whiteKnight.CheckAvailableMoves();
        whiteKnight.AvailableMoves.ForEach(s => Console.WriteLine(s.ToString()));
        DisplayPieces();

        Assert.That(whiteKnight.AvailableMoves.Count, Is.EqualTo(8));
        Assert.That(whiteKnight.AvailableMoves, Does.Contain(new Vector { Y = 2, X = 3 }));
        Assert.That(whiteKnight.AvailableMoves, Does.Contain(new Vector { Y = 2, X = 5 }));
        Assert.That(whiteKnight.AvailableMoves, Does.Contain(new Vector { Y = 3, X = 2 }));
        Assert.That(whiteKnight.AvailableMoves, Does.Contain(new Vector { Y = 3, X = 6 }));
        Assert.That(whiteKnight.AvailableMoves, Does.Contain(new Vector { Y = 5, X = 2 }));
        Assert.That(whiteKnight.AvailableMoves, Does.Contain(new Vector { Y = 5, X = 6 }));
        Assert.That(whiteKnight.AvailableMoves, Does.Contain(new Vector { Y = 6, X = 3 }));
        Assert.That(whiteKnight.AvailableMoves, Does.Contain(new Vector { Y = 6, X = 5 }));
    }

    [Test]
    public void CheckKingMoves1()
    {
        GameInstance.Board = new IPiece[8, 8];
        var queen = new Queen()
            { GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector() { Y = 2, X = 4 } };
        GameInstance.Pieces = new List<IPiece>()
        {
            new King() { GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector { Y = 0, X = 2 } },
            new King() { GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector { Y = 7, X = 2 } },
            new Rook() {GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector { Y = 7, X = 3 } },
            new Rook() {GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector() { Y = 0, X = 7 } },
            new Rook() {GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector() { Y = 0, X = 1 } },
            queen
        };
        GameInstance.Pieces.ForEach(p => GameInstance.Board[p.Position.Y, p.Position.X] = p);
        DisplayPieces();
        GameInstance.Pieces.ForEach(p => p.CheckAvailableMoves());
        var king = GameInstance.Pieces.OfType<King>().First(p => p.PieceColor == EPieceColor.White);
        king.CheckAvailableMoves();
        king.AvailableMoves.ForEach(v => Console.WriteLine("Move: " + v));
        Console.WriteLine("King Moves:" + king.AvailableMoves.Count);
        Console.WriteLine("Queen Moves:" + queen.AvailableMoves.Count);
        queen.AvailableMoves.ForEach(s => Console.WriteLine("Rook Move:" + s));
        Assert.That(king.AvailableMoves.Count, Is.EqualTo(1));
        Assert.That(king.AvailableMoves.Contains(new Vector(){Y=1,X=2}));
        Assert.Pass();
    }

    [Test]
    public void CheckKingMoves2()
    {
        GameInstance.Board = new IPiece[8, 8];
        GameInstance.Pieces = new List<IPiece>()
        {
            new King() { GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector { Y = 4, X = 4 } },
            new King() { GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector { Y = 7, X = 2 } },
            new Rook() {GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector() { Y = 3,X = 3}},
            new Bishop() {GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector() { Y = 7,X = 7}},
        };
        GameInstance.Pieces.ForEach(p => GameInstance.Board[p.Position.Y, p.Position.X] = p);
        DisplayPieces();
        GameInstance.Pieces.ForEach(p => p.CheckAvailableMoves());
        var king = GameInstance.Pieces.OfType<King>().First(p => p.PieceColor == EPieceColor.White);
        king.CheckAvailableMoves();
        king.AvailableMoves.ForEach(v => Console.WriteLine("Move: " + v));
        Console.WriteLine("King Moves:" + king.AvailableMoves.Count);
        Assert.That(king.AvailableMoves.Count, Is.EqualTo(2));
        Assert.That(king.AvailableMoves.Contains(new Vector(){Y = 4, X = 5}));
        Assert.That(king.AvailableMoves.Contains(new Vector(){Y = 5, X = 4}));
        Assert.Pass();
    }

    [Test]
    public void KingAtEdgeNoThreats()
    {
        GameInstance.Board = new IPiece[8, 8];
        GameInstance.Pieces = new List<IPiece>()
        {
            new King() { GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector { Y = 0, X = 0 } },
            new King() { GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector { Y = 7, X = 7 } },
        };
        GameInstance.Pieces.ForEach(p => GameInstance.Board[p.Position.Y, p.Position.X] = p);
        GameInstance.Pieces.ForEach(p => p.CheckAvailableMoves());

        var king = GameInstance.Pieces.OfType<King>().First(p => p.PieceColor == EPieceColor.White);
        Assert.That(king.AvailableMoves.Count, Is.EqualTo(3));
        Assert.That(king.AvailableMoves, Has.Exactly(1).EqualTo(new Vector(){Y = 0, X = 1}));
        Assert.That(king.AvailableMoves, Has.Exactly(1).EqualTo(new Vector(){Y = 1, X = 0}));
        Assert.That(king.AvailableMoves, Has.Exactly(1).EqualTo(new Vector(){Y = 1, X = 1}));
    }

    [Test]
    public void KingSurroundedByFriendlyPieces()
    {
        GameInstance.Board = new IPiece[8, 8];
        var kingPosition = new Vector { Y = 3, X = 3 };

        var friendlyPositions = new List<Vector>
        {
            new Vector{Y=2, X=2}, new Vector{Y=2, X=3}, new Vector{Y=2, X=4},
            new Vector{Y=3, X=2}, /* King */         new Vector{Y=3, X=4},
            new Vector{Y=4, X=2}, new Vector{Y=4, X=3}, new Vector{Y=4, X=4}
        };

        GameInstance.Pieces = new List<IPiece>
        {
            new King() { GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = kingPosition },
            new King() { GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector { Y = 7, X = 7 } }
        };

        GameInstance.Pieces.AddRange(friendlyPositions.Select(pos =>
            new Rook() { GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = pos }));

        GameInstance.Pieces.ForEach(p => GameInstance.Board[p.Position.Y, p.Position.X] = p);
        GameInstance.Pieces.ForEach(p => p.CheckAvailableMoves());

        var king = GameInstance.Pieces.OfType<King>().First(p => p.PieceColor == EPieceColor.White);
        DisplayPieces();
        king.AvailableMoves.ForEach(s => Console.WriteLine(s));
        Assert.That(king.AvailableMoves.Count, Is.EqualTo(0));
    }

    [Test]
    public void KingCanCaptureEnemyPiece()
    {
        GameInstance.Board = new IPiece[8, 8];
        GameInstance.Pieces = new List<IPiece>()
        {
            new King() { GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector { Y = 3, X = 3 } },
            new King() { GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector { Y = 7, X = 7 } },
            new Bishop() { GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector { Y = 2, X = 2 } },
        };

        GameInstance.Pieces.ForEach(p => GameInstance.Board[p.Position.Y, p.Position.X] = p);
        GameInstance.Pieces.ForEach(p => p.CheckAvailableMoves());

        var king = GameInstance.Pieces.OfType<King>().First(p => p.PieceColor == EPieceColor.White);
        Assert.That(king.AvailableMoves, Does.Contain(new Vector() { Y = 2, X = 2 }));
    }

    [Test]
    public void KingAvoidsMovingIntoCheck()
    {
        GameInstance.Board = new IPiece[8, 8];
        GameInstance.Pieces = new List<IPiece>()
        {
            new King() { GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector { Y = 4, X = 4 } },
            new King() { GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector { Y = 7, X = 7 } },
            new Rook() { GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector { Y = 4, X = 6 } },
        };

        GameInstance.Pieces.ForEach(p => GameInstance.Board[p.Position.Y, p.Position.X] = p);
        GameInstance.Pieces.ForEach(p => p.CheckAvailableMoves());

        var king = GameInstance.Pieces.OfType<King>().First(p => p.PieceColor == EPieceColor.White);

        // (4,5) is on the Rook’s line of attack and should be avoided
        Assert.That(king.AvailableMoves, Does.Not.Contain(new Vector(){Y = 4, X = 5}));
    }

    [Test]
    public void KingCannotMoveNextToEnemyKing()
    {
        GameInstance.Board = new IPiece[8, 8];
        GameInstance.Pieces = new List<IPiece>()
        {
            new King() { GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector { Y = 4, X = 4 } },
            new King() { GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector { Y = 5, X = 5 } },
        };
        GameInstance.Pieces.ForEach(p => GameInstance.Board[p.Position.Y, p.Position.X] = p);
        GameInstance.Pieces.ForEach(p => p.CheckAvailableMoves());

        var whiteKing = GameInstance.Pieces.OfType<King>().First(p => p.PieceColor == EPieceColor.White);

        // White king must not be allowed to move adjacent to black king (e.g., 5,5 already occupied)
        Assert.That(whiteKing.AvailableMoves, Does.Not.Contain(new Vector { Y = 5, X = 5 }));
        Assert.That(whiteKing.AvailableMoves, Does.Not.Contain(new Vector { Y = 5, X = 4 }));
        Assert.That(whiteKing.AvailableMoves, Does.Not.Contain(new Vector { Y = 4, X = 5 }));
    }

    [Test]
    public void KingMustBlockCheckOnlyOneMove()
    {
        GameInstance.Board = new IPiece[8, 8];
        GameInstance.Pieces = new List<IPiece>()
        {
            new King() { GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector { Y = 0, X = 4 } },
            new King() { GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector { Y = 7, X = 7 } },
            new Rook() { GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector { Y = 0, X = 7 } }, // checking along row
            new Rook() { GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector { Y = 7, X = 3 } },
            new Rook() { GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector { Y = 7, X = 5 } },
        };
        GameInstance.Pieces.ForEach(p => GameInstance.Board[p.Position.Y, p.Position.X] = p);
        GameInstance.Pieces.ForEach(p => p.CheckAvailableMoves());

        var whiteKing = GameInstance.Pieces.OfType<King>().First(p => p.PieceColor == EPieceColor.White);
        whiteKing.CheckAvailableMoves();
        DisplayPieces();
        whiteKing.AvailableMoves.ForEach(s => Console.WriteLine(s.ToString()));

        Assert.That(whiteKing.AvailableMoves.Count, Is.EqualTo(1));
        Assert.That(whiteKing.AvailableMoves, Does.Contain(new Vector { Y = 1, X = 4 }));
    }

    [Test]
    public void KingIsCheckmated()
    {
        GameInstance.Board = new IPiece[8, 8];
        GameInstance.Pieces = new List<IPiece>()
        {
            new King() { GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector { Y = 0, X = 0 } },
            new King() { GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector { Y = 7, X = 7 } },
            new Queen() { GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector { Y = 0, X = 1 } },
            new Queen() { GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector { Y = 1, X = 0 } },
        };
        GameInstance.Pieces.ForEach(p => GameInstance.Board[p.Position.Y, p.Position.X] = p);
        GameInstance.Pieces.ForEach(p => p.CheckAvailableMoves());
        GameInstance.Pieces.ForEach(p => p.CheckVisibleFields());

        var whiteKing = GameInstance.Pieces.OfType<King>().First(p => p.PieceColor == EPieceColor.White);
        DisplayPieces();

        // All escape squares are covered, it's a checkmate
        Assert.That(whiteKing.AvailableMoves.Count, Is.EqualTo(0));
    }

    [Test]
    public void KingCanCastleKingside()
    {
        GameInstance.Board = new IPiece[8, 8];
        var whiteKing = new King() { GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector { Y = 7, X = 4 } };
        var whiteRook = new Rook() { GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector { Y = 7, X = 7 } };
        GameInstance.Pieces = new List<IPiece>() { whiteKing, whiteRook, new King() { GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector { Y = 0, X = 0 } } };
        GameInstance.Pieces.ForEach(p => GameInstance.Board[p.Position.Y, p.Position.X] = p);
        GameInstance.Pieces.ForEach(p => p.CheckAvailableMoves());

        Assert.That(whiteKing.AvailableMoves, Does.Contain(new Vector { Y = 7, X = 6 }));
    }

    [Test]
    public void PawnCanCaptureEnPassant()
    {
        GameInstance.Board = new IPiece[8, 8];
        var whitePawn = new Pawn(EPieceColor.White) { GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector { Y = 3, X = 4 } };
        var blackPawn = new Pawn(EPieceColor.Black) { GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector { Y = 3, X = 5 } };

        GameInstance.Pieces = new List<IPiece>() {
            whitePawn, blackPawn,
            new King { GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector { Y = 7, X = 0 }},
            new King { GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector { Y = 0, X = 7 }},
        };

        GameInstance.Board[whitePawn.Position.Y, whitePawn.Position.X] = whitePawn;
        GameInstance.Board[blackPawn.Position.Y, blackPawn.Position.X] = blackPawn;

        // Simulate black pawn double move
        blackPawn.IsLastMoveDouble = true;
        DisplayPieces();

        whitePawn.CheckAvailableMoves();
        Assert.That(whitePawn.AvailableMoves, Does.Contain(new Vector { Y = 2, X = 5 }));
    }

    [Test]
    public void GameIsStalemate()
    {
        GameInstance.Board = new IPiece[8, 8];
        var whiteKing = new King() { GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector { Y = 7, X = 7 } };
        var blackKing = new King() { GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector { Y = 0, X = 0 } };
        var blackQueen = new Queen() { GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector { Y = 6, X = 6 } };

        GameInstance.Pieces = new List<IPiece>() { whiteKing, blackKing, blackQueen };
        GameInstance.Pieces.ForEach(p => GameInstance.Board[p.Position.Y, p.Position.X] = p);
        GameInstance.Pieces.ForEach(p => p.CheckAvailableMoves());

        // Assert.That(GameInstance.IsStalemate(EPieceColor.White), Is.True); // Assuming you implemented this TODO
    }

    [Test]
    public void PawnCanBePromoted()
    {
        GameInstance.Board = new IPiece[8, 8];
        var whitePawn = new Pawn(EPieceColor.White)
        {
            GameInstance = GameInstance,
            PieceColor = EPieceColor.White,
            Position = new Vector { Y = 1, X = 0 }
        };

        var whiteKing = new King() { GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = new Vector { Y = 7, X = 4 } };
        var blackKing = new King() { GameInstance = GameInstance, PieceColor = EPieceColor.Black, Position = new Vector { Y = 0, X = 7 } };

        GameInstance.Pieces = new List<IPiece> { whitePawn, whiteKing, blackKing };
        GameInstance.Pieces.ForEach(p => GameInstance.Board[p.Position.Y, p.Position.X] = p);

        whitePawn.CheckAvailableMoves();
        var target = new Vector { Y = 0, X = 0 };
        Assert.That(whitePawn.AvailableMoves, Does.Contain(target));

        // Simulate promotion
        GameInstance.Board[1, 0] = null;
        GameInstance.Board[0, 0] = new Queen { GameInstance = GameInstance, PieceColor = EPieceColor.White, Position = target };
        GameInstance.Pieces.Remove(whitePawn);
        GameInstance.Pieces.Add(GameInstance.Board[0, 0]);

        Assert.That(GameInstance.Board[0, 0], Is.TypeOf<Queen>());
    }





}