namespace Sudoku.PSO;

using Sudoku.Shared;
public class PSOSolver : ISudokuSolver
{
        public SudokuGrid Solve(SudokuGrid s)
        {
            return s.CloneSudoku();
            //s.Cells-->code pso
            //resollution pso
            // s.Cells = recuperation du tableau
            // return s;
        }
    }
