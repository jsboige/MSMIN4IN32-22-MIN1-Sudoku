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
        string sudokuSolved = dSatur.main();
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

    public string main()
    {
        int[,] dataMatrix = null;
        LinkedList<int>[] adjacentVertices = null;

        HashSet<int> colorSet = new HashSet<int>();
        for (int i = 1; i < 10; i++) colorSet.Add(i); //Ajoute les couleurs
        int color = 9;

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
        /*
        int maxDeg = maxDegree(dataMatrix); //Indice du noeud de degré max
        dataMatrix[1, maxDeg] = 1;
        */

        bool uncol = uncolored(dataMatrix); //Vrai s'il existe des noeuds non-colorés sinon faux

        while (uncol)
        {
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
            
            HashSet<int> setuncoladj = setUncolAdj(dataMatrix);
            int index = -1;
            if (setuncoladj.Count == 1)
            {
                index = setuncoladj.First();
            }
            else
            {
                index = setuncoladj.First();
                int currMax = dataMatrix[0, index];
                foreach (int id in setuncoladj)
                {
                    int tempMax = dataMatrix[0, id];
                    if (tempMax > currMax)
                    {
                        index = id;
                    }
                }
            }
            
            int control = -1;
            foreach (int col in colorSet)
            {
                int count = 0;
                foreach (int adj in adjacentVertices[index])
                {
                    if (dataMatrix[1, adj] == col)
                    {
                        break;
                    }
                    else
                    {
                        count++;
                    }
                }
                if (count == adjacentVertices[index].Count)
                {
                    dataMatrix[1, index] = col;
                    control = 1;
                    break;
                }
            }
            if (control == -1)
            {
                
                color += 1;
                colorSet.Add(color);
                dataMatrix[1, index] = color;
                // TO DO
            }
            for (int i = 0; i < dataMatrix.GetLength(1); i++)
            {
                dataMatrix[2, i] = 0;
            }
            uncol = uncolored(dataMatrix);
        }

        string sudokuSolved = "";
        for (int i = 0; i < v; i++) sudokuSolved+= dataMatrix[1, i];
        return sudokuSolved;
    }

    #endregion

}