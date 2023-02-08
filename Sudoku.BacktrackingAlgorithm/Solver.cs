<<<<<<< HEAD
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
=======
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
>>>>>>> 485709152ef35a1aff389d166bf2d5527d5e0007
}

