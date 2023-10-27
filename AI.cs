namespace Gameplay;

public class AI
{
    public Board board;
    public MoveGen moveGen;

    public AI(Board board)
    {
        this.board = board;
        moveGen = new(board);
    }

    public float EvaluatePosition()
    {
        return board.pieceDiff;
    }

    public void SearchMoveTree(MoveTreeNode root)
    {
        
    }
}
