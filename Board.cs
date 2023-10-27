namespace Gameplay;

public class Board
{
    public int size;
    public Piece?[,] grid;

    public Board(int size)
    {
        this.size = size;
        grid = new Piece?[size, size];

        for (int y = 0; y < size; y++)
        {

            for (int x = 0; x < size; x++)
            {
                if ((x + y) % 2 != 0) continue;
                if (y < 3)
                {
                    grid[x, y] = new(x, y, true, false);
                }
                else if (y >= size - 3)
                {
                    grid[x, y] = new(x, y, false, false);
                }
            }
        }
    }

    public void DoMove(Move move)
    {
        var maybePiece = grid[move.x, move.y];
        if (maybePiece is Piece piece)
        {
            grid[piece.x, piece.y] = null;
            piece.x = move.tx;
            piece.y = move.ty;
            grid[piece.x, piece.y] = piece;
        }
        if (move.capturedPiece is Piece capturedPiece)
        {
            grid[capturedPiece.x, capturedPiece.y] = null;
        }
    }

    public void UndoMove(Move move)
    {
        var maybePiece = grid[move.tx, move.ty];
        if (maybePiece is Piece piece)
        {
            grid[piece.x, piece.y] = null;
            piece.x = move.x;
            piece.y = move.y;
            grid[piece.x, piece.y] = piece;
        }
        if (move.capturedPiece is Piece capturedPiece)
        {
            grid[capturedPiece.x, capturedPiece.y] = capturedPiece;
        }
    }

    public List<Move> GetValidMoves(Piece piece)
    {
        var validMoves = new List<Move>();

        if (!piece.isQueen)
        {
            if (CanMakeNormalMove(piece, -1, out var leftMove)) validMoves.Add(leftMove);
            if (CanMakeNormalMove(piece, 1, out var rightMove)) validMoves.Add(rightMove);
            if (CanMakeCaptureMove(piece, -1, out var leftCaptureMove)) validMoves.Add(leftCaptureMove);
            if (CanMakeCaptureMove(piece, 1, out var rightCaptureMove)) validMoves.Add(rightCaptureMove);
        }

        return validMoves;
    }

    public List<Move> GetAllValidMoves()
    {
        var validMoves = new List<Move>();

        for (int y = 0; y < size; y++)
        {

            for (int x = 0; x < size; x++)
            {
                if (!TryGetPiece(x, y, out var piece)) continue;
                validMoves.AddRange(GetValidMoves(piece));
            }
        }

        return validMoves;
    }

    public bool CanMakeNormalMove(Piece piece, int dx, out Move move)
    {
        move = new()
        {
            x = piece.x,
            y = piece.y,
            tx = piece.x + dx,
            ty = piece.y + (piece.team ? 1 : -1)
        };
        return IsValidPos(move.tx, move.ty) && !TryGetPiece(move.tx, move.ty, out _);
    }

    public bool CanMakeCaptureMove(Piece piece, int dx, out Move move)
    {
        var sx = piece.x + dx;
        var sy = piece.y + (piece.team ? 1 : -1);
        move = new()
        {
            x = piece.x,
            y = piece.y,
            tx = piece.x + dx * 2,
            ty = piece.y + (piece.team ? 2 : -2)
        };
        if (!TryGetPiece(sx, sy, out var blockingPiece) || blockingPiece.team == piece.team) return false;
        move.capturedPiece = blockingPiece;
        return 0 <= move.tx && move.tx < size && 0 <= move.ty && move.ty < size;
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

    public string Print()
    {
        var lines = "";
        for (int y = size - 1; y >= 0; y--)
        {
            var line = "";
            for (int x = 0; x < size; x++)
            {
                line += TryGetPiece(x, y, out var piece) ? $"{(piece.team ? 1 : 0)}" : ".";
            }
            lines += line + "\n";
        }
        return lines;
    }

    public bool IsValidPos(int x, int y)
    {
        return !(x < 0 || y < 0 || x >= size || y >= size);
    }
}
