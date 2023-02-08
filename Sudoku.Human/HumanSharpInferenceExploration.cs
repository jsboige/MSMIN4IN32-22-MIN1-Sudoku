using Kermalis.SudokuSolver.Core;
using Sudoku.Shared;


namespace Sudoku.Human;


public class HumanSharpInferenceExploration : ISudokuSolver
{

    public SudokuGrid Solve(SudokuGrid s)
    {

        Puzzle puzzle;
        puzzle = Puzzle.Load(s);

        Solver solver = new Solver(puzzle);
        solver.DoWork();
        var sudokuGrid = solver.Puzzle.getBoard();

        //get cellsnapshot in cell class. delete the last one(parce que text et pas cell)-> pas besoin car dans mainwindow class
        //mettre a zero les cells correspondantes aux  les 2-3 dernieres snapshots
        //passer le nouveau tableau a un algorithme de backtracking
        //initialiser et mettre en place l'algorithme de backtracking
        //renvoyer la solution obtenu par inference et exploration

        return new Shared.SudokuGrid(){Cells = sudokuGrid};

    }

}




