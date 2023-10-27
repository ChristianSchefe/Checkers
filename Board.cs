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
                if (y <= 2)
                {
                    grid[x, y] = new()
                    {
                        x = x,
                        y = y,
                        team = 1,
                        isQueen = false
                    };
                }
                else if (y >= size - 2)
                {
                    grid[x, y] = new()
                    {
                        x = x,
                        y = y,
                        team = 1,
                        isQueen = false
                    };
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
        }

        return validMoves;
    }

    public bool CanMakeNormalMove(Piece piece, int dx, out Move move)
    {
        var tx = piece.x + dx;
        var ty = piece.y + piece.team;
        move = new()
        {
            x = piece.x,
            y = piece.y,
            tx = tx,
            ty = ty
        };
        return 0 <= tx && tx < size && 0 <= ty && ty < size && !TryGetPiece(tx, ty, out _);
    }
    public bool CanMakeCaptureMove(Piece piece, int dx, out Move move)
    {
        var tx = piece.x + dx;
        var ty = piece.y + piece.team;
        move = new()
        {
            x = piece.x,
            y = piece.y,
            tx = tx,
            ty = ty
        };
        return 0 <= tx && tx < size && 0 <= ty && ty < size && !TryGetPiece(tx, ty, out _);
    }

    public bool TryGetPiece(int x, int y, out Piece piece)
    {
        piece = default;
        var maybePiece = grid[x, y];
        if (maybePiece.HasValue)
        {
            piece = maybePiece.Value;
            return true;
        }
        return false;
    }
}
