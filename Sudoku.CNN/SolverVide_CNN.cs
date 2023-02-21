using Sudoku.Shared;

namespace Sudoku.CNN
{
    public class SolverVide_CNN : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid s)
        {
            return s.CloneSudoku();
        }
    }
}
