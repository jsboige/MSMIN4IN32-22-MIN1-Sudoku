from ortools.sat.python import cp_model
import numpy as np

instance = "902005403100063025508407060026309001057010290090670530240530600705200304080041950"

def encode_sudoku(sudoku: np.matrix) -> str:
    '''Transform an integer matrix into an encoded string'''
    return ''.join([''.join(list(r.astype(str))) for r in np.asarray(sudoku)])

def decode_sudoku(sample: str) -> np.matrix:
    '''Transform an encoded puzzle into an integer matrix.'''
    return np.matrix([np.array(list(sample[i:i+9])).astype(np.integer) for i in range(0, len(sample), 9)])   
        

def solve_with_cp(grid: np.matrix) -> (np.matrix):
    '''Solve Sudoku instance (np.matrix) with CP modeling. Returns a tuple with the resulting matrix and the execution time in seconds.'''
    assert grid.shape == (9,9)
    
    grid_size = 9
    region_size = 3 #np.sqrt(grid_size).astype(np.int)
    model = cp_model.CpModel() # Step 1

    # Begin of Step2: Create and initialize variables.
    x = {}
    for i in range(grid_size):
        for j in range(grid_size):
            if grid[i, j] != 0:
                x[i, j] = grid[i, j] # Initial values (values already defined on the puzzle).
            else:
                x[i, j] = model.NewIntVar(1, grid_size, 'x[{},{}]'.format(i,j) ) # Values to be found (variyng from 1 to 9).
    # End of Step 2.

    # Begin of Step3: Values constraints.
    # AllDifferent on rows, to declare that all elements of all rows must be different.
    for i in range(grid_size):
        model.AddAllDifferent([x[i, j] for j in range(grid_size)])

    # AllDifferent on columns, to declare that all elements of all columns must be different.
    for j in range(grid_size):
        model.AddAllDifferent([x[i, j] for i in range(grid_size)])

    # AllDifferent on regions, to declare that all elements of all regions must be different.
    for row_idx in range(0, grid_size, region_size):
        for col_idx in range(0, grid_size, region_size):
            model.AddAllDifferent([x[row_idx + i, j] for j in range(col_idx, (col_idx + region_size)) for i in range(region_size)])
    # End of Step 3.

    solver = cp_model.CpSolver() # Step 4
    status = solver.Solve(model) # Step 5
    result = np.zeros((grid_size, grid_size)).astype(np.int32)

    # Begin of Step 6: Getting values defined by the solver
    for i in range(grid_size):
        for j in range(grid_size):
            result[i,j] = int(solver.Value(x[i,j]))

    # End of Step 6

    return result

r = solve_with_cp(decode_sudoku(instance))
print(r)



