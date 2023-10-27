namespace Gameplay;

public struct Move
{
    public int x;
    public int y;

    public int tx;
    public int ty;
    public Piece? capturedPiece;

    public Move(int x, int y, int tx, int ty, Piece? capturedPiece = null)
    {
        this.x = x;
        this.y = y;
        this.tx = tx;
        this.ty = ty;
        this.capturedPiece = capturedPiece;
    }

    public override readonly string ToString()
    {
        return capturedPiece is Piece piece ? $"{x}, {y} -> {tx}, {ty} (Take {piece.x}, {piece.y})" : $"{x}, {y} -> {tx}, {ty}";
    }
}
