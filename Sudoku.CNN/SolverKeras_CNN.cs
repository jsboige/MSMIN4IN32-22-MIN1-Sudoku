using Keras.Models;
using Sudoku.Shared;


namespace Sudoku.CNN
{
    public class SolverKeras_CNN : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid s)
        {
            var model = Model.LoadModel('saved_model.pb');
            return smart_solve(s, model);
        }

        private SudokuGrid smart_solve(SudokuGrid s, Keras.Models.BaseModel model)
        {
            
            
        }
    }
}
