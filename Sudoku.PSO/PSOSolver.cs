using Sudoku.Shared;

namespace Sudoku.PSO;



public class PSOSolver : ISudokuSolver
{
        public SudokuGrid Solve(SudokuGrid s)
        {   
            //s.Cells --> code pso
            //resolution pso
            //s.Cells = recup tableau
            //return s;
            return s.CloneSudoku();
        }
    }
