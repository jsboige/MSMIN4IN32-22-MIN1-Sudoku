using Kermalis.SudokuSolver.Core;
using Sudoku.Shared;


namespace Sudoku.Human;


public class HumanSharpSolver : ISudokuSolver
{

    public SudokuGrid Solve(SudokuGrid s)
    {

        Puzzle puzzle;
        puzzle = Puzzle.Load(s);
       
        Solver solver = new Solver(puzzle);
        solver.DoWork();
        var sudokuGrid = solver.Puzzle.getBoard();
        
        return new Shared.SudokuGrid() 
        {
            Cells = sudokuGrid
        };

    }

}


        
