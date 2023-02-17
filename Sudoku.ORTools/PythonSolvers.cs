using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Python.Runtime;
using Sudoku.Shared;

namespace Sudoku.ORTools
{




    public class OrToolsPythonSolver : PythonSolverBase
    {

        public override Shared.SudokuGrid Solve(Shared.SudokuGrid s)
        {
            //System.Diagnostics.Debugger.Break();

            //For some reason, the Benchmark runner won't manage to get the mutex whereas individual execution doesn't cause issues
            //using (Py.GIL())
            //{
            // create a Python scopes
            using (PyModule scope = Py.CreateScope())
            {
                // convert the Person object to a PyObject
                PyObject pySudoku = s.Cells.ToPython();

                // create a Python variable "person"
                scope.Set("instance", pySudoku);

                // the person object may now be used in Python
                string code = Resources.PythonOrtoolsSolver_Py;
                scope.Exec(code);
                var result = scope.Get("r");
                var managedResult = result.As<int[][]>();
                 return new Shared.SudokuGrid() { Cells = managedResult };
        
            }
            //}

        }
        
        protected override void InitializePythonComponents()
        {
            InstallPipModule("ortools");
            base.InitializePythonComponents();
        }


    }

    }
