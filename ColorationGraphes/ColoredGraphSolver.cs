using Sudoku.Shared;

namespace ColorationGraphes;

public class ColoredGraphSolver:ISudokuSolver
{
    public int convertIndex((int row, int column) neighbours)
    {
        return neighbours.row * 9 + neighbours.column;
    }
    
    public (int row, int column) revertIndex(int index)
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
                colorAlgo.InitColoring(convertIndex((i, j)), s.Cells[i][j]);
                
                foreach ((int row, int column) neighbours in SudokuGrid.CellNeighbours[i][j])
                {
                    colorAlgo.addEdge(convertIndex((i, j)), convertIndex(neighbours));
                }
            }
        }
        
        string sudokuSolved = colorAlgo.greedyColoring();
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
    public void addEdge(int v, int w)
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
    public string greedyColoring()
    {
        // A temporary array to store the available colors. True
        // value of available[cr] would mean that the color cr is
        // assigned to one of its adjacent vertices
        
        bool[] available = StoreColors();

        // Assign colors to remaining V-1 vertices
        AsignColors(result, available);
        
        /*
        List<ResponseModel> responseList = new List<ResponseModel>();
        // print the result
        for (int u = 0; u < V; u++)
        {
            responseList.Add(new ResponseModel(u, result[u]));
        }
        var json = new JavaScriptSerializer().Serialize(responseList);
        */
        string sudokuSolved = string.Join("", result);
        
        return sudokuSolved;
    }

    private void AsignColors(int[] result, bool[] available)
    {
        for (int u = 1; u < V; u++)
        {
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

            // Find the first available color
            int cr;
            for (cr = 0; cr < V; cr++)
            {
                if (available[cr] == false)
                {
                    break;
                }
            }

            result[u] = cr; // Assign the found color

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
        bool[] available = new bool[V];
        for (int cr = 0; cr < V; cr++)
        {
            available[cr] = false;
        }

        return available;
    }

}