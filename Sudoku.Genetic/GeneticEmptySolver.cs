using Sudoku.Shared;

namespace Sudoku.Genetic
{
	public class GeneticEmptySolver : ISudokuSolver
	{
		public SudokuGrid Solve(SudokuGrid s)
		{
			return s.CloneSudoku();
		}
	}
}