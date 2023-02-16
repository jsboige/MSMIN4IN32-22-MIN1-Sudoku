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
    private SudokuGrid FirstSolver(SudokuGrid s)
    {
        ColoringAlgorithm colorAlgo = new ColoringAlgorithm(81);
        int longTab = 9;

        for (int i = 0; i < longTab; i++)
        {
            for (int j = 0; j < longTab; j++)
            {
                colorAlgo.InitColoring(ConvertIndex((i, j)), s.Cells[i][j]);
                
                foreach ((int row, int column) neighbours in SudokuGrid.CellNeighbours[i][j])
                {
                    colorAlgo.AddEdge(ConvertIndex((i, j)), ConvertIndex(neighbours));
                }
            }
        }
        
        string sudokuSolved = colorAlgo.GreedyColoring();
        SudokuGrid sSolved = SudokuGrid.ReadSudoku(sudokuSolved);

        return sSolved;
    }

    private SudokuGrid SecondSolver(SudokuGrid s)
    {
        DSatur dSatur = new DSatur(s);
        string sudokuSolved = dSatur.initDataMatrix();
        SudokuGrid sSolved = SudokuGrid.ReadSudoku(sudokuSolved);

        return sSolved;
    }
    
    public SudokuGrid Solve(SudokuGrid s)
    {
        //SudokuGrid sSolved = FirstSolver(s);
        SudokuGrid sSolved = SecondSolver(s);
        return sSolved;
    }
}

 public class ColoringAlgorithm
{
    private int V; // No. of vertices
    private List<List<int>> adj;
    private int[] result;
    
    
    //Constructor
    public ColoringAlgorithm(int v)
    {
        V = v;
        result = new int[V];
        adj = new List<List<int>>();
        for (int i = 0; i < v; ++i)
        {
            adj.Add(new List<int>());
        }
    }

    //Function to add an edge into the graph
    public void AddEdge(int v, int w)
    {
        adj[v].Add(w);
        adj[w].Add(v); //Graph is undirected
    }

    // Assigns colors (starting from 0) to all vertices and
    // prints the assignment of colors

    public void InitColoring(int index, int value)
    {
        result[index] = value - 1;
    }
    public string GreedyColoring()
    {
        // A temporary array to store the available colors. True
        // value of available[cr] would mean that the color cr is
        // assigned to one of its adjacent vertices
        int boucle = 0;
        while(boucle < 81 && Array.IndexOf(result, -1) != -1){
            bool[] available = StoreColors();

            // Assign colors to remaining V-1 vertices
            AsignColors(available);
            boucle++;
        }
        
        /*
        List<ResponseModel> responseList = new List<ResponseModel>();
        // print the result
        for (int u = 0; u < V; u++)
        {
            responseList.Add(new ResponseModel(u, result[u]));
        }
        var json = new JavaScriptSerializer().Serialize(responseList);
        */
        for (int u = 0; u < V; u++) result[u]++;
        string sudokuSolved = string.Join("", result);
        return sudokuSolved;
    }

    private void AsignColors(bool[] available)
    {
        for (int u = 0; u < V; u++)
        {
            /*Console.WriteLine("\n---------------");*/
            /* for (int k = 0; k < V; k++) Console.Write(" " + result[k]);*/
            // Process all adjacent vertices and flag their colors
            // as unavailable
            IEnumerator<int> it = adj[u].GetEnumerator();
            while (it.MoveNext())
            {
                int i = it.Current;
                if (result[i] != -1)
                {
                    available[result[i]] = true;
                }
            }
            /*Console.WriteLine("\n");
            for (int k = 0; k < 10; k++) Console.Write(" " + available[k]);*/

            // Find the first available color
            int false_amount = available.Count(cr => cr == false);
            
            if(false_amount==1){
                int cr;
                for (cr = 0; cr < V; cr++)
                {
                    if (available[cr] == false)
                    {
                        break;
                    }
                }
                Console.WriteLine("case "+u+" mise à "+cr);
                result[u] = cr; // Assign the found color
            } else {
                /*Console.WriteLine("raté : "+u+" car couleurs "+false_amount+" possibles");*/
            }
                
            // Reset the values back to false for the next iteration
            it = adj[u].GetEnumerator();
            while (it.MoveNext())
            {
                int i = it.Current;
                if (result[i] != -1)
                {
                    available[result[i]] = false;
                }
            }
        }
    }

    private bool[] StoreColors()
    {
        bool[] available = new bool[9];
        for (int cr = 0; cr < 9; cr++)
        {
            available[cr] = false;
        }

        return available;
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
        // Console.WriteLine("c");
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
                Console.WriteLine("C'EST FINI");
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
                    // Console.WriteLine(index);
                }
            }

            // Check if there is no available colors at all
            // Console.WriteLine("\n");
            // Console.WriteLine("indice actuel : "+index);
            // Console.WriteLine("indice précedent : "+previous_index);
            // Console.WriteLine("Couleur à l'indice précédent :"+dataMatrix[1,previous_index]);
            // Console.WriteLine("indice : "+index);
            // Console.WriteLine("ColorCount :"+colorSetIndex.Count);
            if(colorSetIndex.Count == 9){
                dataMatrix[1,previous_index] = 0;
                for (int i = 0; i < dataMatrix.GetLength(1); i++)
                 {
                     dataMatrix[2, i] = 0;
                 }
                // Console.WriteLine("\n");
                // Console.WriteLine("Indice remis à 0 : "+previous_index);
                // Console.WriteLine(dataMatrix[1,previous_index]);
                // Console.WriteLine("a");
                return;
            } 
            else 
            {
                  for(int i = 1; i < 10 ;i++){
                      if(!colorSetIndex.Contains(i) && solved == false){
                         dataMatrix[1,index] = i;
                        //  Console.WriteLine("couleur :"+i);
                          // Console.WriteLine("case changée en "+dataMatrix[1,index]);
                          // Console.WriteLine("\n");
                        for (int j = 0; j < dataMatrix.GetLength(1); j++)
                        {
                            dataMatrix[2, j] = 0;
                        }
                        RecursiveSolve(dataMatrix,index);
                    }
                    // else {Console.WriteLine("d");}
                        
                }

                if(solved == false){
                 dataMatrix[1,index] = 0;
                }
                // Console.WriteLine("e");
            }

        
    }

    #endregion

}