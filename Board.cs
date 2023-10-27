namespace Gameplay;

public class Board
{
    public int size;
    public Piece?[,] grid;
    public bool playerToMove;

    public Board(int size)
    {
        this.size = size;
        grid = new Piece?[size, size];
        playerToMove = true;
        SetupPieces();
    }

    public void SetupPieces()
    {
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
        if (!TryGetPiece(move.x, move.y, out var piece)) return;

        piece.x = move.tx;
        piece.y = move.ty;
        piece.isQueen |= move.promotePiece;

        foreach (var cpiece in move.capturedPieces) grid[cpiece.x, cpiece.y] = null;

        grid[move.x, move.y] = null;
        grid[move.tx, move.ty] = piece;
    }

    public void UndoMove(Move move)
    {
        if (!TryGetPiece(move.tx, move.ty, out var piece)) return;

        piece.x = move.x;
        piece.y = move.y;
        piece.isQueen &= !move.promotePiece;

        foreach (var cpiece in move.capturedPieces) grid[cpiece.x, cpiece.y] = cpiece;

        grid[move.tx, move.ty] = null;
        grid[move.x, move.y] = piece;
    }

    public List<Move> GetAllValidMoves(bool team)
    {
        var validMoves = new List<Move>();

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if (!TryGetPiece(x, y, out var piece) || piece.team != team) continue;
                validMoves.AddRange(GetAllCaptureMoves(x, y));
            }
        }

        if (validMoves.Count > 0) return validMoves;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if (!TryGetPiece(x, y, out var piece) || piece.team != team) continue;
                if (CanMakeNormalMove(piece, -1, out var leftMove)) validMoves.Add(leftMove);
                if (CanMakeNormalMove(piece, 1, out var rightMove)) validMoves.Add(rightMove);
            }
        }
        return validMoves;
    }

    public List<Move> GetAllCaptureMoves(int x, int y)
    {
        var moves = new List<Move>();
        if (!TryGetPiece(x, y, out var piece)) return moves;

        if (CanMakeCaptureMove(piece, -1, 1, out var leftUpCaptureMove)) moves.AddRange(ExtendCaptureMove(leftUpCaptureMove));
        if (CanMakeCaptureMove(piece, 1, 1, out var rightUpCaptureMove)) moves.AddRange(ExtendCaptureMove(rightUpCaptureMove));
        if (CanMakeCaptureMove(piece, -1, -1, out var leftDownCaptureMove)) moves.AddRange(ExtendCaptureMove(leftDownCaptureMove));
        if (CanMakeCaptureMove(piece, 1, -1, out var rightDownCaptureMove)) moves.AddRange(ExtendCaptureMove(rightDownCaptureMove));

        return moves;
    }

    public List<Move> ExtendCaptureMove(Move prevMove)
    {
        DoMove(prevMove);

        var moves = GetAllCaptureMoves(prevMove.tx, prevMove.ty);
        var extendedMoves = new List<Move>();

        foreach (var move in moves)
        {
            var m = new Move(prevMove.x, prevMove.y, move.tx, move.ty, prevMove.capturedPieces);
            m.capturedPieces.AddRange(move.capturedPieces);
            extendedMoves.Add(m);
        }

        UndoMove(prevMove);

        if (extendedMoves.Count == 0)
        {
            extendedMoves.Add(prevMove);
            return extendedMoves;
        }

        var newExtendedMoves = new List<Move>();
        foreach (var move in extendedMoves)
        {
            newExtendedMoves.AddRange(ExtendCaptureMove(move));
        }
        return newExtendedMoves;
    }

    public bool CanMakeNormalMove(Piece piece, int dx, out Move move)
    {
        move = new(piece.x, piece.y, piece.x + dx, piece.y + (piece.team ? 1 : -1), new(), false);
        move.promotePiece = !piece.isQueen && piece.team ? move.ty == size - 1 : move.ty == 0;
        return IsValidPos(move.tx, move.ty) && !TryGetPiece(move.tx, move.ty, out _);
    }

    public bool CanMakeCaptureMove(Piece piece, int dx, int dy, out Move move)
    {
        var sx = piece.x + dx;
        var sy = piece.y + dy;
        move = new(piece.x, piece.y, piece.x + dx * 2, piece.y + dy * 2, new(), false);
        move.promotePiece = !piece.isQueen && piece.team ? move.ty == size - 1 : move.ty == 0;
        if (!TryGetPiece(sx, sy, out var blockingPiece) || blockingPiece.team == piece.team) return false;
        move.capturedPieces = new List<Piece>() { blockingPiece };
        return IsValidPos(move.tx, move.ty) && !TryGetPiece(move.tx, move.ty, out _);
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
                line += TryGetPiece(x, y, out var piece) ? piece.SimplePrint() : "__";
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
