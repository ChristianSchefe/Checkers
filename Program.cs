using Gameplay;

var random = new Random();
var board = new Board(8);

for (int i = 0; i < 500; i++)
{
    var moves = board.GetAllValidMoves();
    if (moves.Count == 0) break;
    var randomMove = moves[random.Next(moves.Count)];
    board.DoMove(randomMove);
    Console.WriteLine(board.Print());
}
