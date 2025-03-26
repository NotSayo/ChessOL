using System.Diagnostics.CodeAnalysis;

namespace Shared.Types;

public struct Vector : IEquatable<Vector>
{
    public int X { get; set; }
    public int Y { get; set; }

    public Vector(int y, int x)
    {
        X = x;
        Y = y;
    }


    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is Vector vector &&
               X == vector.X &&
               Y == vector.Y;
    }

    public bool Equals(Vector other)
    {
        return X == other.X && Y == other.Y;
    }

    public static bool operator ==(Vector left, Vector right)
    {
        return left.X == right.X && left.Y == right.Y;
    }

    public static bool operator !=(Vector left, Vector right)
    {
        return !(left == right);
    }

    public override string ToString()
    {
        return "(" + Y + ", " + X + ")";
    }
}