namespace GeneticSharp
{
    /// <summary>
    /// Validates compatibility between a chromosome's declared gene ordering and the genetic operators
    /// that will be applied to it, so mismatches fail before any generation runs instead of deep inside
    /// a permutation operator mid-run.
    /// </summary>
    public static class GeneticAlgorithmValidator
    {
        /// <summary>
        /// Validates that the crossover and mutation operators are compatible with the chromosome's
        /// declared <see cref="GeneOrdering"/>.
        /// </summary>
        /// <param name="chromosome">The chromosome to validate against.</param>
        /// <param name="crossover">The crossover operator.</param>
        /// <param name="mutation">The mutation operator.</param>
        public static void ValidateOperatorCompatibility(IChromosome chromosome, ICrossover crossover, IMutation mutation)
        {
            ExceptionHelper.ThrowIfNull("chromosome", chromosome);
            ExceptionHelper.ThrowIfNull("crossover", crossover);
            ExceptionHelper.ThrowIfNull("mutation", mutation);

            if (chromosome.GeneOrdering < crossover.RequiredOrdering)
            {
                throw new System.InvalidOperationException(
                    "Crossover {0} requires {1} gene ordering, but chromosome {2} declares {3}.".With(
                        crossover.GetType().Name, crossover.RequiredOrdering, chromosome.GetType().Name, chromosome.GeneOrdering));
            }

            if (chromosome.GeneOrdering < mutation.RequiredOrdering)
            {
                throw new System.InvalidOperationException(
                    "Mutation {0} requires {1} gene ordering, but chromosome {2} declares {3}.".With(
                        mutation.GetType().Name, mutation.RequiredOrdering, chromosome.GetType().Name, chromosome.GeneOrdering));
            }
        }
    }
}
