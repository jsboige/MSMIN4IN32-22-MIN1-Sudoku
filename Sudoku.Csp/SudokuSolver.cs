using Sudoku.Shared;

namespace Sudoku.Csp;

public class SudokuSolver : ISudokuSolver
{
    

    private SudokuGrid grid;

    public SudokuSolver(SudokuGrid grid){
        this.grid = grid;
    }

    public SudokuGrid Solve(SudokuGrid s)
    {
        s = grid;
        return s.CloneSudoku();
    }

    public bool cspSolver(){
        Tuple<int, int> emptyCell = FindEmptyCell();
        if (emptyCell == null)
            return true;

        int row = emptyCell.Item1;
        int col = emptyCell.Item2;

        for (int i = 1; i <= 9; i++)
        {
            if (IsValidMove(row, col, i))
            {
                grid.Cells[row][col] = i;

                if (cspSolver())
                    return true;

                grid.Cells[row][col] = 0;
            }
        }

        return false;
    }
    
       private Tuple<int, int> FindEmptyCell()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (grid.Cells[i][j] == 0)
                    return Tuple.Create(i, j);
            }
        }
        return null;
    }
    

    private bool IsValidMove(int row, int col, int num)
    {
        return !UsedInRow(row, num) && !UsedInCol(col, num) &&
               !UsedInSubgrid(row, col, num);
    }

    private bool UsedInRow(int row, int num)
    {
        for (int i = 0; i < 9; i++)
            if (grid.Cells[row][i] == num)
                return true;
        return false;
    }

    private bool UsedInCol(int col, int num)
    {
        for (int i = 0; i < 9; i++)
            if (grid.Cells[i][col] == num)
                return true;
        return false;
    }

    private bool UsedInSubgrid(int row, int col, int num)
    {
        int startRow = row - row % 3;
        int startCol = col - col % 3;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (grid.Cells[i + startRow][j + startCol] == num)
                    return true;
            }
        }
        return false;
    }
}