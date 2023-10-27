namespace Gameplay;

public class MoveGen
{
    public Board board;

    public MoveGen(Board board)
    {
        this.board = board;
    }

    public List<MoveTreeNode> GenerateMoveTrees(bool team)
    {
        var validMoves = GenerateCaptureMoveTrees(team);
        if (validMoves.Count == 0) validMoves = GenerateSimpleMoveTrees(team);
        return validMoves;
    }

    public List<MoveTreeNode> GenerateSimpleMoveTrees(bool team)
    {
        int dy = team ? 1 : -1;
        int promotionY = team ? board.size - 1 : 0;
        var moves = new List<MoveTreeNode>();
        for (int y = 0; y < board.size; y++)
        {
            for (int x = 0; x < board.size; x++)
            {
                if (!board.TryGetPiece(x, y, out var piece) || piece.team != team) continue;
                if (!piece.isQueen)
                {
                    var moveNW = new Move(x, y, x - 1, y + dy, y == promotionY);
                    if (board.IsValidMove(moveNW)) moves.Add(new(moveNW, new()));

                    var moveNE = new Move(x, y, x + 1, y + dy, y == promotionY);
                    if (board.IsValidMove(moveNE)) moves.Add(new(moveNE, new()));
                }
                else
                {
                    for (int i = 0; i < board.size; i++)
                    {
                        var moveNW = new Move(x, y, x - i, y + dy * i);
                        if (board.IsValidMove(moveNW)) moves.Add(new(moveNW, new()));
                        else break;
                    }
                    for (int i = 1; i < board.size; i++)
                    {
                        var moveNE = new Move(x, y, x + i, y + dy * i);
                        if (board.IsValidMove(moveNE)) moves.Add(new(moveNE, new()));
                        else break;
                    }
                }
            }
        }
        return moves;
    }

    public List<MoveTreeNode> GenerateCaptureMoveTrees(bool team)
    {
        var moves = new List<MoveTreeNode>();
        for (int y = 0; y < board.size; y++)
        {
            for (int x = 0; x < board.size; x++)
            {
                if (!board.TryGetPiece(x, y, out var piece) || piece.team != team) continue;
                moves.AddRange(GetCaptureMoves(x, y));
            }
        }
        return moves;
    }

    public MoveTreeNode BuildNodeTree(MoveTreeNode root)
    {
        board.DoMove(root.move);
        root.followUpMoves = GetCaptureMoves(root.move.tx, root.move.ty);
        board.UndoMove(root.move);
        return root;
    }

    public List<MoveTreeNode> GetCaptureMoves(int x, int y)
    {
        var moves = new List<MoveTreeNode>();
        if (!board.TryGetPiece(x, y, out var piece)) return moves;
        if (!piece.isQueen)
        {
            var promotionY = piece.team ? board.size - 1 : 0;

            var moveNW = new Move(x, y, x - 2, y + 2, y + 2 == promotionY);
            if (board.IsValidMove(moveNW) && board.TryGetPiece(x - 1, y + 1, out var capturedPieceNW) && capturedPieceNW.team != piece.team)
            {
                moveNW.capturedPiece = capturedPieceNW;
                moves.Add(new MoveTreeNode(moveNW, new()));
            }

            var moveNE = new Move(x, y, x + 2, y + 2, y + 2 == promotionY);
            if (board.IsValidMove(moveNE) && board.TryGetPiece(x + 1, y + 1, out var capturedPieceNE) && capturedPieceNE.team != piece.team)
            {
                moveNE.capturedPiece = capturedPieceNE;
                moves.Add(new MoveTreeNode(moveNE, new()));
            }

            var moveSW = new Move(x, y, x - 2, y - 2, y - 2 == promotionY);
            if (board.IsValidMove(moveSW) && board.TryGetPiece(x - 1, y - 1, out var capturedPieceSW) && capturedPieceSW.team != piece.team)
            {
                moveSW.capturedPiece = capturedPieceSW;
                moves.Add(new MoveTreeNode(moveSW, new()));
            }

            var moveSE = new Move(x, y, x + 2, y - 2, y - 2 == promotionY);
            if (board.IsValidMove(moveSE) && board.TryGetPiece(x + 1, y - 1, out var capturedPieceSE) && capturedPieceSE.team != piece.team)
            {
                moveSE.capturedPiece = capturedPieceSE;
                moves.Add(new MoveTreeNode(moveSE, new()));
            }
        }
        else
        {
            var moveNW = new Move(x, y, x - 2, y + 2, false);
            if (board.IsValidMove(moveNW) && board.TryGetPiece(x - 1, y + 1, out var capturedPieceNW) && capturedPieceNW.team != piece.team)
            {
                moveNW.capturedPiece = capturedPieceNW;
                moves.Add(new MoveTreeNode(moveNW, new()));
            }

            var moveNE = new Move(x, y, x + 2, y + 2, false);
            if (board.IsValidMove(moveNE) && board.TryGetPiece(x + 1, y + 1, out var capturedPieceNE) && capturedPieceNE.team != piece.team)
            {
                moveNE.capturedPiece = capturedPieceNE;
                moves.Add(new MoveTreeNode(moveNE, new()));
            }

            var moveSW = new Move(x, y, x - 2, y - 2, false);
            if (board.IsValidMove(moveSW) && board.TryGetPiece(x - 1, y - 1, out var capturedPieceSW) && capturedPieceSW.team != piece.team)
            {
                moveSW.capturedPiece = capturedPieceSW;
                moves.Add(new MoveTreeNode(moveSW, new()));
            }

            var moveSE = new Move(x, y, x + 2, y - 2, false);
            if (board.IsValidMove(moveSE) && board.TryGetPiece(x + 1, y - 1, out var capturedPieceSE) && capturedPieceSE.team != piece.team)
            {
                moveSE.capturedPiece = capturedPieceSE;
                moves.Add(new MoveTreeNode(moveSE, new()));
            }
        }
        return new(moves.Select(BuildNodeTree));
    }
}

public struct MoveTreeNode
{
    public Move move;
    public List<MoveTreeNode> followUpMoves;

    public MoveTreeNode(Move move, List<MoveTreeNode> followUpMoves)
    {
        this.move = move;
        this.followUpMoves = followUpMoves;
    }
}
