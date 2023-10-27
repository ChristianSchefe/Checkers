using Gameplay;

var random = new Random();
var board = new Board(8);
var movegen = new MoveGen(board);
var leafCount = 0;

// board.SetupPieces();
board.grid[0, 0] = new(0, 0, true, false);

for (int x = 1; x < board.size; x += 2)
{
    for (int y = 1; y < board.size; y += 2)
    {
        board.grid[x, y] = new(x, y, false, false);
    }
}

board.Print();

var trees = movegen.GenerateMoveTrees(true);
foreach (var move in trees) PrintMoveTree(move);
Console.WriteLine(leafCount);
// for (int i = 0; i < 10; i++)
// {
//     var moveTrees = movegen.GenerateMoveTrees((i % 2) == 0);
//     Console.WriteLine(moveTrees.Count);
//     if (moveTrees.Count == 0) break;
//     var randomTree = moveTrees[random.Next(moveTrees.Count)];
//     PrintMoveTree(randomTree);
//     ExecuteMoveTree(randomTree);

//     board.Print();
// }

void ExecuteMoveTree(MoveTreeNode tree)
{
    board.DoMove(tree.move);
    // foreach (var move in tree.followUpMoves) ExecuteMoveTree(tree);
    if (tree.followUpMoves.Count > 0) ExecuteMoveTree(tree.followUpMoves[0]);
}

void PrintMoveTree(MoveTreeNode tree)
{
    board.DoMove(tree.move);

    board.Print();
    if (tree.followUpMoves.Count == 0) leafCount++;
    foreach (var move in tree.followUpMoves) PrintMoveTree(move);
    Console.WriteLine("................");
    board.UndoMove(tree.move);
}
