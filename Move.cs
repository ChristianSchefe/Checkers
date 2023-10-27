namespace Gameplay;

public struct Move
{
    public int x;
    public int y;

    public int tx;
    public int ty;
    public List<Piece> capturedPieces;
    public bool promotePiece;

    public Move(int x, int y, int tx, int ty, List<Piece> capturedPieces, bool promotePiece = false)
    {
        this.x = x;
        this.y = y;
        this.tx = tx;
        this.ty = ty;
        this.capturedPieces = capturedPieces;
        this.promotePiece = promotePiece;
    }

    public override readonly string ToString()
    {
        var moveStr = $"{x}, {y} -> {tx}, {ty}";
        if (promotePiece) moveStr += " (Queen)";
        foreach (var piece in capturedPieces) moveStr += $" (Take {piece.x}, {piece.y})";
        return moveStr;
    }
}
