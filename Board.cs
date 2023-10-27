namespace Gameplay;

public class Board
{
    public int size;
    public Piece?[,] grid;
    public bool playerToMove;
    public int pieceDiff;

    public Board(int size)
    {
        this.size = size;
        grid = new Piece?[size, size];
        playerToMove = true;
        pieceDiff = 0;
    }

    public void SetupPieces()
    {
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if ((x + y) % 2 != 0) continue;
                if (y < 3) grid[x, y] = new(x, y, true, false);
                else if (y >= size - 3) grid[x, y] = new(x, y, false, false);
            }
        }
    }

    public void DoMove(Move move)
    {
        if (!TryGetPiece(move.x, move.y, out var piece)) return;

        piece.x = move.tx;
        piece.y = move.ty;
        piece.isQueen |= move.promotePiece;

        if (move.capturedPiece is Piece cpiece)
        {
            grid[cpiece.x, cpiece.y] = null;
            pieceDiff += piece.team ? 1 : -1;
        }

        grid[move.x, move.y] = null;
        grid[move.tx, move.ty] = piece;
    }

    public void UndoMove(Move move)
    {
        if (!TryGetPiece(move.tx, move.ty, out var piece)) return;

        piece.x = move.x;
        piece.y = move.y;
        piece.isQueen &= !move.promotePiece;

        if (move.capturedPiece is Piece cpiece)
        {
            grid[cpiece.x, cpiece.y] = cpiece;
            pieceDiff -= piece.team ? 1 : -1;
        }
        grid[move.tx, move.ty] = null;
        grid[move.x, move.y] = piece;
    }

    public void DoFullMove(FullMove move)
    {
        for (int i = 0; i < move.moves.Count; i++) DoMove(move.moves[i]);
        playerToMove = !playerToMove;
    }

    public void UndoFullMove(FullMove move)
    {
        for (int i = move.moves.Count - 1; i >= 0; i--) UndoMove(move.moves[i]);
        playerToMove = !playerToMove;
    }


    public bool IsValidMove(Move move)
    {
        if (!IsValidPos(move.x, move.y) || !IsValidPos(move.tx, move.ty)) return false;
        if (!HasPiece(move.x, move.y) || HasPiece(move.tx, move.ty)) return false;
        return true;
    }

    public bool TryGetPiece(int x, int y, out Piece piece)
    {
        piece = default;
        if (!IsValidPos(x, y)) return false;
        var maybePiece = grid[x, y];
        if (maybePiece.HasValue)
        {
            piece = maybePiece.Value;
            return true;
        }
        return false;
    }

    public bool HasPiece(int x, int y) => grid[x, y].HasValue;

    public bool IsValidPos(int x, int y) => x >= 0 && y >= 0 && x < size && y < size;

    public void Print()
    {
        var lines = "";
        for (int y = size - 1; y >= 0; y--)
        {
            var line = "";
            for (int x = 0; x < size; x++)
            {
                line += TryGetPiece(x, y, out var piece) ? piece.SimplePrint() : "__";
            }
            lines += line + "\n";
        }
        Console.WriteLine(lines);
    }
}
