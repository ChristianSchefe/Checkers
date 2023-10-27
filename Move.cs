namespace Gameplay;

public struct Move
{
    public int x, y, tx, ty;
    public Piece? capturedPiece;
    public bool promotePiece;

    public Move(int x, int y, int tx, int ty, bool promotePiece = false)
    {
        this.x = x;
        this.y = y;
        this.tx = tx;
        this.ty = ty;
        this.promotePiece = promotePiece;
        capturedPiece = null;
    }
}

public struct FullMove
{
    public List<Move> moves;

    public FullMove(List<Move> moves)
    {
        this.moves = moves;
    }

    public override readonly string ToString()
    {
        if (moves.Count <= 0) return "";
        var s = $"({moves[0].x + 1} {moves[0].y + 1})";
        foreach (var move in moves)
        {
            s += $"->({move.tx + 1} {move.ty + 1})";
        }
        return s;
    }
}
