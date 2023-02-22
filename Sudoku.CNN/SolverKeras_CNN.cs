using System;
using Keras.Applications;
using Numpy;
using Sudoku.Shared;
using System.Diagnostics;
using System.IO;
using Numpy.Models;

namespace Sudoku.CNN
{
    public class SolverKeras_CNN : PythonSolverBase
    {

		protected override void InitializePythonComponents()
		{
			InstallPipModule("tensorflow");
			InstallPipModule("keras");
			base.InitializePythonComponents();
		}

		public override SudokuGrid Solve(SudokuGrid s)
        {

            //string filepath = Path.Combine(Environment.CurrentDirectory, "variables");
            string filepath = "C:\\Users\\Lysandre\\Documents\\GitHub\\MSMIN4IN32-22-MIN1-Sudoku\\Sudoku.CNN\\sudoku.model";
            //if (!File.Exists(filepath))
            //{
				//File.WriteAllBytes(filepath, Resource1.variables);
			//}
			
			Keras.Models.BaseModel model = Keras.Models.Model.LoadModel(filepath);
            return Simple_solver(s, model);
        }

        private SudokuGrid Simple_solver(SudokuGrid s, Keras.Models.BaseModel model)
        {
            //Conversion de la SudokuGrid en array Numpy pour l'injection dans le CNN
            double[,] SudokuBuffer = new double[9, 9];

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    SudokuBuffer[i, j] = s.Cells[i][j];
                    SudokuBuffer[i, j] /= 9;
                    SudokuBuffer[i, j] -= 0.5;
                }
            }

            var input = np.array(SudokuBuffer);
            while (true)
            {
                //Prediction du CNN
                var output = model.Predict(input.reshape(1, 9, 9, 1));
                output = output.squeeze();
                var pred = np.argmax(output, axis: 1).reshape(9, 9) + 1;
                var proba = np.around(np.max(output, axis: new[] { 1 }).reshape(9, 9), 2);
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        input[i][j] += 0.5;
                        input[i][j] *= 9;
                    }
                }
                var mask = input.@equals(0);
                if (((int)mask.sum()) == 0)
                {
                    break;
                }
                var probNew = proba * mask;
                var ind = (int)np.argmax(probNew);
                var (x, y) = ((ind / 9), ind % 9);
                var val = pred[x][y];
                input[x][y] = val;
                Console.WriteLine(input.ToString());
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        input[i][j] /= 9;
                        input[i][j] -= 0.5;
                    }
                }
            }
            SudokuGrid sol = new SudokuGrid();
            
            //Reconversion de la solution en SudokuGrid
            for(int i = 0; i < 9; i++)
            {
                for (int j = 0; j<9; j++)
                {
                    sol.Cells[i] = input[i].astype(np.int32).GetData<int>();
                }
            }

            return sol;
        }

        
    }
}
