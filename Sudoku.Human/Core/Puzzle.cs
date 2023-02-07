using Kermalis.SudokuSolver.Core;
using Sudoku.Shared;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace Sudoku.Human.Core;

internal sealed class Puzzle
{
    public readonly ReadOnlyCollection<Region> Rows;
    public readonly ReadOnlyCollection<Region> Columns;
    public readonly ReadOnlyCollection<Region> Blocks;
    public readonly ReadOnlyCollection<ReadOnlyCollection<Region>> Regions;

    public readonly BindingList<string> Actions;
    public readonly bool IsCustom;
    /// <summary>Stored as x,y (col,row)</summary>
    private readonly Cell[][] _board;

    public Cell this[int x, int y] => _board[x][y];

    public Puzzle(int[][] board, bool isCustom)
    {
        IsCustom = isCustom;
        Actions = new BindingList<string>();

        _board = new Cell[9][];
        for (int x = 0; x < 9; x++)
        {
            _board[x] = new Cell[9];
            for (int y = 0; y < 9; y++)
            {
                _board[x][y] = new Cell(this, board[x][y], new SPoint(x, y));
            }
        }

        var rows = new Region[9];
        var columns = new Region[9];
        var blocks = new Region[9];
        for (int i = 0; i < 9; i++)
        {
            var cells = new Cell[9];
            int c;

            for (c = 0; c < 9; c++)
            {
                cells[c] = _board[c][i];
            }
            rows[i] = new Region(cells);

            for (c = 0; c < 9; c++)
            {
                cells[c] = _board[i][c];
            }
            columns[i] = new Region(cells);

            c = 0;
            int ix = i % 3 * 3;
            int iy = i / 3 * 3;
            for (int x = ix; x < ix + 3; x++)
            {
                for (int y = iy; y < iy + 3; y++)
                {
                    cells[c++] = _board[x][y];
                }
            }
            blocks[i] = new Region(cells);
        }

        Regions = new ReadOnlyCollection<ReadOnlyCollection<Region>>(new ReadOnlyCollection<Region>[3]
        {
            Rows = new ReadOnlyCollection<Region>(rows),
            Columns = new ReadOnlyCollection<Region>(columns),
            Blocks = new ReadOnlyCollection<Region>(blocks)
        });

        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                _board[x][y].CalcVisibleCells();
            }
        }
    }

    public void RefreshCandidates()
    {
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                Cell cell = this[x, y];
                for (int i = 1; i <= 9; i++)
                {
                    cell.Candidates.Add(i);
                }
            }
        }
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                Cell cell = this[x, y];
                if (cell.Value != Cell.EMPTY_VALUE)
                {
                    cell.Set(cell.Value);
                }
            }
        }
    }

    public static Puzzle Load(SudokuGrid s)
    {

        int[][] board = new int[9][];
        for (int col = 0; col < 9; col++)
        {
            board[col] = new int[9];
        }

        board = s.Cells;

        return new Puzzle(board, false);
    }

    public int[][] getBoard()
    {
        int[][] board = new int[9][];
        for (int col = 0; col < 9; col++)
        {
            board[col] = new int[9];
        }

        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                Cell cell = this[x, y];
                board[x][y] = cell.Value;
            }
        }

        return board;
    }


    public static string TechniqueFormat(string technique, string format, params object[] args)
    {
        return string.Format(string.Format("{0,-20}", technique) + format, args);
    }

    public void LogAction(string action)
    {
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                Cell cell = this[x, y];
                cell.CreateSnapshot(false, false);
            }
        }
        Actions.Add(action);
    }
    public void LogAction(string action, Cell culprit, Cell? semiCulprit)
    {
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                Cell cell = this[x, y];
                cell.CreateSnapshot(culprit == cell, semiCulprit == cell);
            }
        }
        Actions.Add(action);
    }
    public void LogAction(string action, IEnumerable<Cell>? culprits, Cell? semiCulprit)
    {
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                Cell cell = this[x, y];
                cell.CreateSnapshot(culprits is not null && culprits.Contains(cell), semiCulprit == cell);
            }
        }
        Actions.Add(action);
    }
    public void LogAction(string action, Cell culprit, IEnumerable<Cell>? semiCulprits)
    {
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                Cell cell = this[x, y];
                cell.CreateSnapshot(culprit == cell, semiCulprits is not null && semiCulprits.Contains(cell));
            }
        }
        Actions.Add(action);
    }
    public void LogAction(string action, IEnumerable<Cell>? culprits, IEnumerable<Cell>? semiCulprits)
    {
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                Cell cell = this[x, y];
                cell.CreateSnapshot(culprits is not null && culprits.Contains(cell), semiCulprits is not null && semiCulprits.Contains(cell));
            }
        }
        Actions.Add(action);
    }
}
}
