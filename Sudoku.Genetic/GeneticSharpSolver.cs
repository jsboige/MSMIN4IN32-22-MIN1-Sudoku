using Sudoku.Shared;

namespace Sudoku.Genetic;

//rediriger les .extensions vers code perso
//remplacer grille sudoku geneticsharp par grille code prof

public class GeneticSharpSolver : ISudokuSolver
{
    public SudokuGrid Solve(SudokuGrid s)
    {
        var sudokuAsString = s.ToString();
        //var sudokuSharp =  GeneticSharp.Extensions.SudokuGrid.Parse(sudokuAsString); //indiquer grille sudoku code prof
        
        var sudokuChromosome = new SudokuCellsChromosome(s); //reindiquer
        var fitness = new SudokuFitness(s); //reindiquer 
        var selection = new GeneticSharp.EliteSelection();
        var crossover = new GeneticSharp.UniformCrossover();
        var mutation = new GeneticSharp.UniformMutation();
        var termination = new GeneticSharp.OrTermination(
            new GeneticSharp.GenerationNumberTermination(200),
            new GeneticSharp.FitnessThresholdTermination(1.0)
        );

        var population = new GeneticSharp.Population(500,500, sudokuChromosome);

        var ga = new GeneticSharp.GeneticAlgorithm(population, fitness, selection, crossover, mutation);
        ga.Termination = termination;
        ga.MutationProbability = 0.1f;
        ga.CrossoverProbability = 0.9f;
        // ga.GenerationRan += (sender, e) =>
        // {
        //     Console.WriteLine($"Generation {e.Generation.Number} - Fitness: {e.Generation.BestChromosome.Fitness}");
        // };

        ga.Start();

        var bestChromosome = ga.BestChromosome as SudokuCellsChromosome; //
        var bestSudoku =  bestChromosome.TargetSudokuBoard;
        //var bestSudokuAsString = bestSudoku.ToString();
        //var bestSudokuGrid = SudokuGrid.ReadSudoku(bestSudokuAsString);
        return bestSudoku;
    }

}
