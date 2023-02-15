using Sudoku.Shared;
using System;
using System.Linq;

namespace Sudoku.Csp;

public class SudokuSolver {
    private const int GRID_SIZE = 9;
    private const int SUBGRID_SIZE = 3;
    private int[,] grid;

    public SudokuSolver(int[,] grid)
    {
        this.grid = grid;
    }

    public bool Solve()
    {
        Tuple<int, int> emptyCell = FindEmptyCell();
        if (emptyCell == null)
            return true;

        int row = emptyCell.Item1;
        int col = emptyCell.Item2;

        for (int i = 1; i <= GRID_SIZE; i++)
        {
            if (IsValidMove(row, col, i))
            {
                grid[row, col] = i;

                if (Solve())
                    return true;

                grid[row, col] = 0;
            }
        }

        return false;
    }

    private Tuple<int, int> FindEmptyCell()
    {
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                if (grid[i, j] == 0)
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
        for (int i = 0; i < GRID_SIZE; i++)
            if (grid[row, i] == num)
                return true;
        return false;
    }

    private bool UsedInCol(int col, int num)
    {
        for (int i = 0; i < GRID_SIZE; i++)
            if (grid[i, col] == num)
                return true;
        return false;
    }

    private bool UsedInSubgrid(int row, int col, int num)
    {
        int startRow = row - row % SUBGRID_SIZE;
        int startCol = col - col % SUBGRID_SIZE;

        for (int i = 0; i < SUBGRID_SIZE; i++)
        {
            for (int j = 0; j < SUBGRID_SIZE; j++)
            {
                if (grid[i + startRow, j + startCol] == num)
                    return true;
            }
        }
        return false;
    }
}