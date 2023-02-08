using Sudoku.Shared;

namespace Sudoku.Csp;

public class SudokuSolver : ISudokuSolver
{
    public SudokuGrid Solve(SudokuGrid s)
        {
            return s.CloneSudoku();
        }
}