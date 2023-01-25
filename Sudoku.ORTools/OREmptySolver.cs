namespace Sudoku.ORTools;
public class OREmptySolver
{
  public SudokuGrid Solve(SudokuGrid s)
        {
            return s.CloneSudoku();
        }
}
