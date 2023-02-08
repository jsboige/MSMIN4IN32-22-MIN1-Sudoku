using Sudoku.Shared;
namespace Sudoku.BacktrackingAlgorithm
{
    public class Solver: ISudokuSolver
    {	
        public SudokuGrid Solve(SudokuGrid s)
            {
                return s.CloneSudoku();
            } 
    }
}

