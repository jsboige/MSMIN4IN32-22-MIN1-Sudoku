using Python.Runtime;
using Sudoku.Shared;

namespace Sudoku.Genetic;


public class GeneticPythonSolver : PythonSolverBase
{


    public override Shared.SudokuGrid Solve(Shared.SudokuGrid s)
    {

        //using (Py.GIL())
        //{
        // create a Python scope
        using (PyModule scope = Py.CreateScope())
        {
            // convert the Person object to a PyObject
            PyObject pyCells = s.Cells.ToPython();

            // create a Python variable "person"
            scope.Set("instance", pyCells);

            // the person object may now be used in Python
            string code = PythonScripts.GeneticPython_py;
            scope.Exec(code);
            var result = scope.Get("r");
            var managedResult = result.As<int[][]>();
            //var convertesdResult = managedResult.Select(objList => objList.Select(o => (int)o).ToArray()).ToArray();
            return new Shared.SudokuGrid() { Cells = managedResult };
        }
        //}

    }

    protected override void InitializePythonComponents()
    {
        //1InstallPipModule("z3-solver");
        base.InitializePythonComponents();
    }



}