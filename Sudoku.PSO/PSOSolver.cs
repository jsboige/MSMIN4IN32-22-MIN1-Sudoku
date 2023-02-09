using Sudoku.Shared;

namespace Sudoku.PSO;



public class PSOSolver : ISudokuSolver
{
        public SudokuGrid Solve(SudokuGrid s)
        {   
            int[,] output = new int[9,9];
            //s.Cells --> code pso
            for(int i=0; i<9; i++){
                for(int j=0; j<9; j++){
                    output[i,j] = s.Cells[i][j];
                }
            }
            Console.Write("Sudoku be like :");
            Console.Write(output.ToString());
            //resolution pso
            //s.Cells = recup tableau
            //return s;
            return s.CloneSudoku();
        }
    }
