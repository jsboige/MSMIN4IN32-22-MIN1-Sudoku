using Sudoku.Shared;

namespace Sudoku.Genetic;

public class HumanSharpSolver : ISudokuSolver
{
    public SudokuGrid Solve(SudokuGrid s)
    {
        return s.CloneSudoku();
    }

}
