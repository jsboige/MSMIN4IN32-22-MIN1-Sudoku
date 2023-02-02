using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Sudoku.Human.Core;

namespace Kermalis.SudokuSolver.Core;

internal sealed partial class Solver
{
	public Puzzle Puzzle { get; }

	public Solver(Puzzle puzzle)
	{
		Puzzle = puzzle;
	}

	public void DoWork()
	{
		Puzzle.RefreshCandidates();
		

		bool solved; // If this is true after a segment, the puzzle is solved and we can break
		do
		{
			solved = true;
			bool changed = CheckForNakedSinglesOrCompletion(ref solved);

			// Solved or failed to solve
			if (solved
				|| (!changed && !RunTechnique()))
			{
				break;
			}
		} while (true);

	}
	private bool CheckForNakedSinglesOrCompletion(ref bool solved)
	{
		bool changed = false;
		for (int x = 0; x < 9; x++)
		{
			for (int y = 0; y < 9; y++)
			{
				Cell cell = Puzzle[x, y];
				if (cell.Value == Cell.EMPTY_VALUE)
				{
					solved = false;
					// Check for naked singles
					HashSet<int> a = cell.Candidates;
					if (a.Count == 1)
					{
						int nakedSingle = a.ElementAt(0);
						cell.Set(nakedSingle);
						
						changed = true;
					}
				}
			}
		}
		return changed;
	}

	private bool RunTechnique()
	{
		foreach (SolverTechnique t in _techniques)
		{
			if (t.Function.Invoke(Puzzle))
			{
				return true;
			}
		}
		return false;
	}
}
