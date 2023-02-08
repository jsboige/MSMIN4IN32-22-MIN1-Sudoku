namespace Sudoku.BacktrackingAlgorithm;
//test
public class Solver
{
    public class GeneticEmptySolver : ISudokuSolver
	{
		public SudokuGrid Solve(SudokuGrid s)
		{
			return s.CloneSudoku();
		}
	}
}
