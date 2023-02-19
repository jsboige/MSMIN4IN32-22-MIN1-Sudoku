using Sudoku.Shared;

namespace Sudoku.Human
{
    public class HumanEmptySolver : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid s)
        {
            return s.CloneSudoku();
        }
    }
}