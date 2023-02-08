using Kermalis.SudokuSolver.Core;
using Sudoku.Shared;
using System.Runtime.InteropServices;


namespace Sudoku.Human;


public class HumanSharpInferenceExploration : ISudokuSolver
{

    public SudokuGrid Solve(SudokuGrid s)
    {
        bool full = false;
        Puzzle puzzle;
        puzzle = Puzzle.Load(s);

        Solver solver = new Solver(puzzle);
        solver.DoWork();
        var sudokuGrid = solver.Puzzle.getBoard();
        full = solver.Puzzle.Rows.All(row => row.All(c => c.Value != 0));
        if (!full)
        {
            int N = sudokuGrid.GetLength(0);
            solveSudoku(sudokuGrid, N);
        }
            

        //get cellsnapshot in cell class. delete the last one(parce que text et pas cell)-> pas besoin car dans mainwindow class
        //mettre a zero les cells correspondantes aux  les 2-3 dernieres snapshots
        //passer le nouveau tableau a un algorithme de backtracking
        //initialiser et mettre en place l'algorithme de backtracking
        //renvoyer la solution obtenu par inference et exploration

        return new Shared.SudokuGrid(){Cells = sudokuGrid};
    }

    public static bool isSafe(int[][] board,
                            int row, int col,
                            int num)
    {

        // Row has the unique (row-clash)
        for (int d = 0; d < board.GetLength(0); d++)
        {

            // Check if the number
            // we are trying to
            // place is already present in
            // that row, return false;
            if (board[row][d] == num)
            {
                return false;
            }
        }

        // Column has the unique numbers (column-clash)
        for (int r = 0; r < board.GetLength(0); r++)
        {

            // Check if the number
            // we are trying to
            // place is already present in
            // that column, return false;
            if (board[r][col] == num)
            {
                return false;
            }
        }

        // corresponding square has
        // unique number (box-clash)
        int sqrt = (int)Math.Sqrt(board.GetLength(0));
        int boxRowStart = row - row % sqrt;
        int boxColStart = col - col % sqrt;

        for (int r = boxRowStart;
             r < boxRowStart + sqrt; r++)
        {
            for (int d = boxColStart;
                 d < boxColStart + sqrt; d++)
            {
                if (board[r][d] == num)
                {
                    return false;
                }
            }
        }

        // if there is no clash, it's safe
        return true;
    }

    public static bool solveSudoku(int[][] board,
                                           int n)
    {
        int row = -1;
        int col = -1;
        bool isEmpty = true;
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (board[i][j] == 0)
                {
                    row = i;
                    col = j;

                    // We still have some remaining
                    // missing values in Sudoku
                    isEmpty = false;
                    break;
                }
            }
            if (!isEmpty)
            {
                break;
            }
        }

        // no empty space left
        if (isEmpty)
        {
            return true;
        }

        // else for each-row backtrack
        for (int num = 1; num <= n; num++)
        {
            if (isSafe(board, row, col, num))
            {
                board[row][col] = num;
                if (solveSudoku(board, n))
                {

                    // Print(board, n);
                    return true;
                }
                else
                {

                    // Replace it
                    board[row][col] = 0;
                }
            }
        }
        return false;
    }
//    var minCandidates = allCells.Min(cell => cell.Candidates.Count > 0 ? cell.Candidates.Count : int.MaxValue);

//                    if (minCandidates != int.MaxValue)
//                    {
//                        // Utilisation de l'heuristique Deg: de celles qui ont le moins de candidats à égalité, on choisi celle la plus contraignante, celle qui a le plus de voisins (on pourrait faire mieux avec le nombre de candidats en commun avec ses voisins)
//                        var candidateCells = allCells.Where(cell => cell.Candidates.Count == minCandidates);
//    //var degrees = candidateCells.Select(candidateCell => new {Cell = candidateCell, Degree = candidateCell.GetCellsVisible().Aggregate(0, (sum, neighbour) => sum + neighbour.Candidates.Count) });
//    var degrees = candidateCells.Select(candidateCell => new { Cell = candidateCell, Degree = candidateCell.GetCellsVisible().Count(c => c.Value == 0) }).ToList();
//    //var targetCell = list_cell.First(cell => cell.Candidates.Count == minCandidates);
//    var maxDegree = degrees.Max(deg1 => deg1.Degree);
//    var targetCell = degrees.First(deg => deg.Degree == maxDegree).Cell;

//    //dernière exploration pour ne pas se mélanger les pinceaux

//    BackTrackingState currentlyExploredCellValues;
//                        if (exploredCellValues.Count == 0 || !exploredCellValues.Peek().Cell.Equals(targetCell))
//                        {
//                            currentlyExploredCellValues = new BackTrackingState() { Board = monPuzzle.GetBoard(), Cell = targetCell, ExploredValues = new List<int>() };
//    exploredCellValues.Push(currentlyExploredCellValues);
//                        }
//                        else
//                        {
//                            currentlyExploredCellValues = exploredCellValues.Peek();
//                        }


//                        //utilisation de l'heuristique LCV: on choisi la valeur la moins contraignante pour les voisins
//                        var candidateValues = targetCell.Candidates.Where(i => !currentlyExploredCellValues.ExploredValues.Contains(i));
//var neighbourood = targetCell.GetCellsVisible();
//var valueConstraints = candidateValues.Select(v => new
//{
//    Value = v,
//    ContraintNb = neighbourood.Count(neighboor => neighboor.Candidates.Contains(v))
//}).ToList();
//var minContraints = valueConstraints.Min(vc => vc.ContraintNb);
//var exploredValue = valueConstraints.First(vc => vc.ContraintNb == minContraints).Value;
//currentlyExploredCellValues.ExploredValues.Add(exploredValue);
//targetCell.Set(exploredValue);
//                        //targetCell.Set(exploredValue, true);

//                    }
//                    else
//{
//    //Plus de candidats possibles, on atteint un cul-de-sac
//    if (monPuzzle.IsValid())
//    {
//        solved = true;
//    }
//    else
//    {
//        deadEnd = true;
//    }


//    //deadEnd = true;
//}
}




