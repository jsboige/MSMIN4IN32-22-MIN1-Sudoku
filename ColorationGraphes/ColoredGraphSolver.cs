using Sudoku.Shared;

namespace ColorationGraphes;

public class ColoredGraphSolver:ISudokuSolver
{
    private int ConvertIndex((int row, int column) neighbours)
    {
        return neighbours.row * 9 + neighbours.column;
    }
    private (int row, int column) RevertIndex(int index)
    {
        return ((index - index % 9) / 9, index % 9);
    }
    public SudokuGrid Solve(SudokuGrid s)
    {
        DSatur dSatur = new DSatur(s);
        string sudokuSolved = dSatur.initDataMatrix();
        SudokuGrid sSolved = SudokuGrid.ReadSudoku(sudokuSolved);
        return sSolved;
    }
}

class DSatur
{
    SudokuGrid s;
    LinkedList<int>[] adjacentVertices = null;
    HashSet<int> colorSet = new HashSet<int>();
    int color = 9;
    bool solved = false;
    public DSatur(SudokuGrid s)
    {
        this.s = s;
    }

    #region METODE
    
    private int ConvertIndex((int row, int column) neighbours)
    {
        return neighbours.row * 9 + neighbours.column;
    }
    private (int row, int column) RevertIndex(int index)
    {
        return ((index - index % 9) / 9, index % 9);
    }

    public int maxDegree(int[,] arr)
    {
        int max = arr[0, 0];
        int indexMax = 0;
        for (int i = 1; i < arr.GetLength(1); i++)
        {
            if (arr[0, i] > max)
            {
                max = arr[0, i];
                indexMax = i;
            }
        }
        return indexMax;
    }

    public bool uncolored(int[,] arr)
    {
        for (int i = 0; i < arr.GetLength(1); i++)
        {
            if (arr[1, i] == 0)
            {
                return true;
            }
        }
        return false;
    }

    public HashSet<int> setUncolAdj(int[,] arr)
    {
        HashSet<int> set = new HashSet<int>();
        int max = arr[2, 0];
        for (int i = 0; i < arr.GetLength(1); i++)
        {
            if (arr[2, i] > max)
            {
                max = arr[2, i]; // Nombre de couleurs voisines de i
            }
        }
        for (int i = 0; i < arr.GetLength(1); i++)
        {
            if (arr[2, i] >= max)
            {
                set.Add(i); // Ajout d'un index à l'ensemble
            }
        }
        return set;
    }
    
    public string initDataMatrix(){

        int[,] dataMatrix = null;

        for (int i = 1; i < 10; i++) colorSet.Add(i); //Ajoute les couleurs


        #region Chargement du Graphe

        int v = 81; // Nombre de noeuds
        dataMatrix = new int[3, v];
        adjacentVertices = new LinkedList<int>[v];
        for (int i = 0; i < v; i++) adjacentVertices[i] = new LinkedList<int>();

        int longTab = 9;
        for (int i = 0; i < longTab; i++)
        {
            for (int j = 0; j < longTab; j++)
            {
                dataMatrix[1, ConvertIndex((i, j))] = s.Cells[i][j];

                foreach ((int row, int column) neighbours in SudokuGrid.CellNeighbours[i][j])
                {
                    int vertex1 = ConvertIndex((i, j));
                    int vertex2 = ConvertIndex(neighbours);

                    adjacentVertices[vertex1].AddLast(vertex2);
                }
            }
        }

        #endregion

        for (int i = 0; i < v; i++)
        {
            dataMatrix[0, i] = adjacentVertices[i].Count;
        }

        RecursiveSolve(dataMatrix, 0);
        string sudokuSolved = "";
        for (int i = 0; i < v; i++) sudokuSolved+= dataMatrix[1, i];
        return sudokuSolved;
    }
    public void RecursiveSolve(int[,] dataMatrix, int previous_index)
    {
            // Check if sudoku is solved
            int zero_count = 0;
            for (int i = 0; i < dataMatrix.GetLength(1); i++)
            {
                if(dataMatrix[1,i] == 0) zero_count++;
                    
            }
            if(zero_count == 0){
                solved = true;
                return;
            }

            // get number of unavailable colors for each index
            for (int i = 0; i < dataMatrix.GetLength(1); i++)
            {
                if (dataMatrix[1, i] == 0)
                {
                    HashSet<int> temp = new HashSet<int>();
                    foreach (int w in adjacentVertices[i])
                    {
                        temp.Add(dataMatrix[1, w]);
                    }
                    dataMatrix[2, i] = temp.Count;
                }
            }
            
            // Get list of unavailable colors for most restricted index
            HashSet<int> setuncoladj = setUncolAdj(dataMatrix);
            int index = -1;
            index = setuncoladj.First();
            
            HashSet<int> colorSetIndex = new HashSet<int>();
            foreach (int w in adjacentVertices[index])
            {
                if(dataMatrix[1, w] != 0){
                    colorSetIndex.Add(dataMatrix[1, w]);
                }
            }

            // Check if there is no available colors at all
            if(colorSetIndex.Count == 9){
                dataMatrix[1,previous_index] = 0;
                for (int i = 0; i < dataMatrix.GetLength(1); i++)
                 {
                     dataMatrix[2, i] = 0;
                 }
                return;
            } 
            else 
            {
                  for(int i = 1; i < 10 ;i++){
                      if(!colorSetIndex.Contains(i) && solved == false){
                         dataMatrix[1,index] = i;
                        for (int j = 0; j < dataMatrix.GetLength(1); j++)
                        {
                            dataMatrix[2, j] = 0;
                        }
                        RecursiveSolve(dataMatrix,index);
                    }
                        
                }

                if(solved == false){
                 dataMatrix[1,index] = 0;
                }
            }

        
    }

    #endregion

}