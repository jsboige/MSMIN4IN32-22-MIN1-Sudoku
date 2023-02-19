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
            using System;
            using System.Linq;
            using System.Numerics;

            public static int[][][] BatchSmartSolve(int[][][] grids, Model solver)
            {
                int[][][] result = new int[grids.Length][][];
                grids.CopyTo(result, 0);

                int numBlanks = grids.Select(g => g.SelectMany(row => row).Count(x => x == 0)).Max();

                for (int i = 0; i < numBlanks; i++)
                {
                    float[][][] preds = solver.Predict(ToCategorical(result));
                    float[][] probs = preds.Select(p => p.Select(row => row.Max()).ToArray()).ToArray();
                    int[][] values = preds.Select(p => p.Select(row => Array.IndexOf(row, row.Max()) + 1).ToArray()).ToArray();
                    bool[][] blanks = result.Select(g => g.SelectMany(row => row).Select(x => x == 0).ToArray()).ToArray();

                    for (int j = 0; j < result.Length; j++)
                    {
                        if (blanks[j].Any(b => b))
                        {
                            int[] blankIndices = blanks[j].Select((b, k) => new { b, k }).Where(x => x.b).Select(x => x.k).ToArray();
                            int[] confidences = blankIndices.Select(k => probs[j][k] == 0 ? -1 : Array.IndexOf(preds[j][k], preds[j][k].Max())).ToArray();
                            int[] bestConfidences = blankIndices.Select(k => confidences[k] == -1 ? -1 : confidences.Where(c => c >= 0).OrderByDescending(c => probs[j][k]).First()).ToArray();
                            int[] bestValues = blankIndices.Select(k => bestConfidences[k] == -1 ? 0 : values[j][k][bestConfidences[k]]).ToArray();

                            for (int k = 0; k < blankIndices.Length; k++)
                            {
                                if (bestConfidences[k] >= 0)
                                {
                                    int[] fillIndices = new int[] { blankIndices[k] % 9, blankIndices[k] / 9 };
                                    result[j][fillIndices[1]][fillIndices[0]] = bestValues[k];
                                }
                            }
                        }
                    }
                }

                return result;
            }

            private static float[][][] ToCategorical(int[][][] grids)
            {
                float[][][] result = new float[grids.Length][][];
                for (int i = 0; i < grids.Length; i++)
                {
                    result[i] = new float[9][];
                    for (int j = 0; j < 9; j++)
                    {
                        result[i][j] = new float[81];
                        for (int k = 0; k < 81; k++)
                        {
                            result[i][j][k] = grids[i][k / 9][k % 9] == j + 1 ? 1 : 0;
                        }
                    }
                }
                return result;
            }


        }
    }
}
