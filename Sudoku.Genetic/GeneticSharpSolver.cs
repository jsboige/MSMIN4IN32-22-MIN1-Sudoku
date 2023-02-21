using GeneticSharp;
using GeneticSharp.Extensions;
using Sudoku.Shared;

namespace Sudoku.Genetic;

//rediriger les .extensions vers code perso
//remplacer grille sudoku geneticsharp par grille code prof

public abstract class GeneticSharpSolverBase : ISudokuSolver
{
    public SudokuGrid Solve(SudokuGrid s)
    {
        //var sudokuAsString = s.ToString();
        //var sudokuSharp =  GeneticSharp.Extensions.SudokuGrid.Parse(sudokuAsString); //indiquer grille sudoku code prof

        var populationSize = 200;
		SudokuGrid toReturn = null;
		do
        {
	        var sudokuChromosome = GetChromosome(s); //reindiquer
	        var fitness = new SudokuFitness(s); //reindiquer 
	        var selection = new GeneticSharp.EliteSelection();
	        var crossover = new GeneticSharp.UniformCrossover();
	        var mutation = new GeneticSharp.UniformMutation();
	        var termination = new GeneticSharp.OrTermination(
		        new GeneticSharp.GenerationNumberTermination(200),
		        new GeneticSharp.FitnessThresholdTermination(0)
	        );

	        var population = new GeneticSharp.Population(populationSize, populationSize, (IChromosome)sudokuChromosome);

	        var ga = new GeneticSharp.GeneticAlgorithm(population, fitness, selection, crossover, mutation);
	        ga.Termination = termination;
	        ga.MutationProbability = 0.1f;
	        ga.CrossoverProbability = 0.9f;
	        // ga.GenerationRan += (sender, e) =>
	        // {
	        //     Console.WriteLine($"Generation {e.Generation.Number} - Fitness: {e.Generation.BestChromosome.Fitness}");
	        // };

	        ga.Start();

	        var bestChromosome = ga.BestChromosome as ISudokuChromosome; //
	        toReturn = bestChromosome.GetSudokus()[0];
			//var bestSudokuAsString = bestSudoku.ToString();
			//var bestSudokuGrid = SudokuGrid.ReadSudoku(bestSudokuAsString);
			populationSize = populationSize * 5;
		} while (toReturn.IsValid(s) == false);


		return toReturn;
    }

    protected abstract ISudokuChromosome GetChromosome(SudokuGrid sudokuGrid);

}


public class GeneticSharpCellsSolver : GeneticSharpSolverBase
{
	protected override ISudokuChromosome GetChromosome(SudokuGrid sudokuGrid)
	{
		return new SudokuCellsChromosome(sudokuGrid);
	}
}

public class GeneticSharpPermutationSolver : GeneticSharpSolverBase
{
	protected override ISudokuChromosome GetChromosome(SudokuGrid sudokuGrid)
	{
		return new SudokuPermutationsChromosome(sudokuGrid);
	}
}

public class GeneticSharpRandomPermutationSolver : GeneticSharpSolverBase
{
	private int _NbPermutations = 5;
	private int _NbSudokus = 5;
	protected override ISudokuChromosome GetChromosome(SudokuGrid sudokuGrid)
	{
		return new SudokuRandomPermutationsChromosome(sudokuGrid, _NbPermutations, _NbSudokus);
	}
}