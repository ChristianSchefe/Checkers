using Gameplay;

var random = new Random();
var board = new Board(8);

// board.grid[2, 0] = new(2, 0, true, false);
// board.grid[1, 1] = new(1, 1, false, false);
// board.grid[3, 1] = new(3, 1, false, false);
// board.grid[5, 3] = new(5, 3, false, false);

// Console.WriteLine(board.Print());

// var moves = board.GetAllCaptureMoves(2, 0);
// foreach(var move in moves) Console.WriteLine(move);

for (int i = 0; i < 30; i++)
{
    var moves = board.GetAllValidMoves((i % 2) == 0);
    if (moves.Count == 0) break;
    var randomMove = moves[random.Next(moves.Count)];
    board.DoMove(randomMove);
    Console.WriteLine(randomMove);
    Console.WriteLine(board.Print());
}
