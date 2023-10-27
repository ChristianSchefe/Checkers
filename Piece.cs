namespace Gameplay;

public struct Piece
{
    public int x;
    public int y;
    public bool team;
    public bool isQueen;

    public Piece(int x, int y, bool team, bool isQueen) {
        this.x = x;
        this.y = y;
        this.team = team;
        this.isQueen = isQueen;
    }
}
