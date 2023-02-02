using Sudoku.Shared;


namespace Sudoku.Human;

public class HumanSharpSolver : ISudokuSolver
{
    public SudokuGrid Solve(SudokuGrid s)
    {
        Solver solver = new Solver(new Puzzle(s, true));
        solver.doWork();
        
    }

}
