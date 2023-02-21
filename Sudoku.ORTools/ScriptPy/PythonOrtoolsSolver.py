import PythonOrtoolsSolver as cp_model
from ortools.sat.python import cp_model
import numpy as np
"""
instance = ((0,0,0,0,9,4,0,3,0),
            (0,0,0,5,1,0,0,0,7),
            (0,8,9,0,0,0,0,4,0),
            (0,0,0,0,0,0,2,0,8),
            (0,6,0,2,0,1,0,5,0),
            (1,0,2,0,0,0,0,0,0),
            (0,7,0,0,0,0,5,2,0),
            (9,0,0,0,6,5,0,0,0),
            (0,4,0,9,7,0,0,0,0))
"""
#r = instance


#instance  = np.random.randint(10, size=(9,9))

instance = "300200000000107000706030500070009080900020004010800050009040301000702000000008006"
print(instance)
print("\n\nPremier Tour\n\n")

#instance = np.asmatrix(instance)
"""
#for i in range(9):
    #for j in range (9):
        #table = np.random.randint(9, size=10)
        #table[j] = np.random.randint(9, size=10)



def encode_sudoku(sudoku: np.matrix) -> str:
    '''Transform an integer matrix into an encoded string'''
    return ''.join([''.join(list(r(str))) for r in np.asarray(sudoku)])

"""
def decode_sudoku(sample: str) -> np.matrix:
    '''Transform an encoded puzzle into an integer matrix.'''
    return np.matrix([np.array(list(sample[i:i+9])).astype(np.int32) for i in range(0, len(sample), 9)])

instance = decode_sudoku(instance)
print('', + instance)


def solve_with_cp(grid: np.matrix) -> (np.matrix):

    #Verifier si la taille de notre tableau de sudoku est 9*9
    #assert grid.shape == (9,9) 
    grid_size = 9
    #taille d'un block
    region_size = 3
    model = cp_model.CpModel()
    
    #Create and initialize variables
    table = {}
    for i in range(grid_size):
        for j in range(grid_size):
            if (grid[i,j] != 0):
                #put in the table the values w already have on the puzzle at the beggining
                table[i,j] = grid[i,j]
            else:
                #The values that we will found varies between 1 and 9
                table[i,j]= model.NewIntVar(1, grid_size, 'table[{},{}]'.format(i,j))
    

    #Constraints

    #All values in rows are differents  and between 1 to 9
    for i in range (grid_size):
        model.AddAllDifferent(
            [table[i,j] for j in range(grid_size)]
                            )

    #All values in col are differents  and between 1 to 9
    for j in range (grid_size):
        model.AddAllDifferent(
            [table[i,j] for i in range(grid_size)]
                            )

    
    # AllDifferent on regions, to declare that all elements of all regions must be different.
    for row_idx in range(0, grid_size, region_size):
        for col_idx in range(0, grid_size, region_size):
            model.AddAllDifferent([table[row_idx + i, j] for j in range(col_idx, (col_idx + region_size)) for i in range(region_size)])
    # End of Step 3.

    solver = cp_model.CpSolver() # Step 4
    status = solver.Solve(model) # Step 5
    result = np.zeros((grid_size, grid_size)).astype(np.int32)

    # Begin of Step 6: Getting values defined by the solver
    if status == cp_model.FEASIBLE:
        for i in range(grid_size):
            for j in range(grid_size):
                result[i,j] = int(solver.Value(table[i,j]))
    
    # End of Step 6
        
    return result

print('\n\n', + solve_with_cp(instance)) 
