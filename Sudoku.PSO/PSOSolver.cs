using Sudoku.Shared;

namespace Sudoku.PSO;



public class PSOSolver : ISudokuSolver
{
        public SudokuGrid Solve(SudokuGrid s)
        {   
            int[,] output = new int[9,9];
            //s.Cells --> code pso
            Console.Write("Sudoku be like :");
            for(int i=0; i<9; i++){
                Console.Write("\r\n ");
                for(int j=0; j<9; j++){
                    Console.Write(s.Cells[i][j].ToString());
                }
            }
            //resolution pso
            //s.Cells = recup tableau
            //return s;
            return s.CloneSudoku();
    }
}