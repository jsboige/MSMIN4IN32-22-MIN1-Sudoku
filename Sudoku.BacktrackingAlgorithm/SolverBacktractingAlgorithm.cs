
using Sudoku.Shared;
namespace Sudoku.BacktrackingAlgorithm
{
    public class SolverBacktrackingAlgorithm: ISudokuSolver
    {	

        public SudokuGrid Solve(SudokuGrid s) {
            //launch the solver
            backtrack(s, 0, 0);
            return s;
        }
        private bool backtrack(SudokuGrid s, int row, int col){   
            //pass to the next row if all the column are checked     
            if(col==9)
            {
                col = 0; ++row;
                if(row==9) return true;
            }
            //check if the column is filled
            if(s.Cells[row][col]!=0)
                return backtrack(s, row, col+1);
            //implement the good value
            for(int v = 1 ; v <=9 ; v++)
            {                
                if(isValid(s, row, col, v))
                {                
                    s.Cells[row][col] = v;
                    if(backtrack(s, row,col+1)) return true;                    
                    else s.Cells[row][col] = 0;
                }
            }  
            return false;
        }
        private bool isValid(SudokuGrid s, int row, int col,int val){
            //check if value is present in column
            for(int r = 0 ; r < 9 ; r++)
                if(s.Cells[r][col]==val) return false;
                
            //check if value is present in the row
            for(int c = 0 ; c < 9 ; c++)
                if(s.Cells[row][c]==val) return false;    
            
            //check for the value in the 3 X 3 block
            int I = row/3; int J = col/3;
            for(int a = 0 ; a < 3 ; a++)
                for(int b = 0 ; b < 3 ; b++)
                    if(val==s.Cells[3*I+a][3*J+b]) return false;
            
            return true; 
        }    
    }
}

