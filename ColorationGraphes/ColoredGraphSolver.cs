using Sudoku.Shared;

namespace ColorationGraphes;

public class ColoredGraphSolver:ISudokuSolver
{
    private int ConvertIndex((int row, int column) neighbours)
    {
        return neighbours.row * 9 + neighbours.column;
    }
    
    public (int row, int column) RevertIndex(int index)
    {
        return ((index - index % 9) / 9, index % 9);
    }
    
    public SudokuGrid Solve(SudokuGrid s)
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
}

 public class ColoringAlgorithm
{
    public int V; // No. of vertices
    public List<List<int>> adj;
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