using Keras.Applications;
using Sudoku.Shared;
using System.Diagnostics;

namespace Sudoku.CNN
{
    public class SolverKeras_CNN : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid s)
        {
            string filepath = "saved_model.pb";
            Keras.Models.BaseModel model = Keras.Models.BaseModel.LoadModel(filepath);
            return Simple_solver(s, model);
        }

        private SudokuGrid Simple_solver(SudokuGrid s, Keras.Models.BaseModel model)
        {
            
        }

        
    }
}
