using System.Collections.Generic;

namespace GeneticSharp
{
    /// <summary>
    /// Defines an operators base strategy to be inherited either with linear or parallel execution
    /// </summary>
    public abstract class OperatorsStrategyBase : IOperatorsStrategy
    {
        /// <summary>
        /// Crosses the specified parents.
        /// </summary>
        /// <param name="population">the population from which parents are selected</param>
        /// <param name="crossover">The crossover class.</param>
        /// <param name="crossoverProbability">The crossover probability.</param>
        /// <param name="parents">The parents.</param>
        /// <returns>The result chromosomes.</returns>
        public abstract IList<IChromosome> Cross(IPopulation population, ICrossover crossover, float crossoverProbability, IList<IChromosome> parents);

        /// <summary>
        /// Mutate the specified chromosomes.
        /// </summary>
        /// <param name="mutation">The mutation class.</param>
        /// <param name="mutationProbability">The mutation probability.</param>
        /// <param name="chromosomes">The chromosomes.</param>
        public  abstract void Mutate(IMutation mutation, float mutationProbability, IList<IChromosome> chromosomes);


        /// <summary>
        /// form parent matches and either executes the corresponding crossover with given probability to produce new children, or returns null children
        /// </summary>
        /// <param name="population">the population from which parents are selected</param>
        /// <param name="crossover">The crossover class.</param>
        /// <param name="crossoverProbability">The crossover probability.</param>
        /// <param name="parents">The parents.</param>
        /// <param name="firstParentIndex">the index of the first parent selected for a crossover</param>
        /// <returns>children for the current crossover if it was performed, null otherwise</returns>
        protected static IList<IChromosome> SelectParentsAndCross(IPopulation population, ICrossover crossover,
            float crossoverProbability, IList<IChromosome> parents, int firstParentIndex)
        {
            var parentsNumber = crossover.ParentsNumber;

            // Checks if there are enough parents left to select, because in the end of the list
            // we can have some rest chromosomes that don't fill a full parent set.
            if (parents.Count - firstParentIndex < parentsNumber)
            {
                return null;
            }

            // If match the probability cross is made, otherwise the offspring is an exact copy of the parents.
            if (RandomizationProvider.Current.GetDouble() <= crossoverProbability)
            {
                var selectedParents = new List<IChromosome>(parentsNumber);

                for (int i = firstParentIndex; i < firstParentIndex + parentsNumber; i++)
                {
                    selectedParents.Add(parents[i]);
                }

                return crossover.Cross(selectedParents);
            }

            return null;
        }

    }
}