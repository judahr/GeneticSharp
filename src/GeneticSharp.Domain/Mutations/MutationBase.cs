namespace GeneticSharp
{
    /// <summary>
    /// Base class for IMutation's implementation.
    /// </summary>
    public abstract class MutationBase : IMutation
    {
        #region Properties
        /// <summary>
        /// Gets or sets the gene ordering a chromosome must declare (or exceed) for this mutation to be valid on it.
        /// </summary>
        public GeneOrdering RequiredOrdering { get; protected set; }
        #endregion

        #region Methods
        /// <summary>
        /// Mutate the specified chromosome.
        /// </summary>
        /// <param name="chromosome">The chromosome.</param>
        /// <param name="probability">The probability to mutate each chromosome.</param>
        public void Mutate(IChromosome chromosome, float probability)
        {
            ExceptionHelper.ThrowIfNull("chromosome", chromosome);

            if (chromosome.GeneOrdering < RequiredOrdering)
            {
                throw new MutationException(
                    this, "{0} requires {1} gene ordering, but chromosome {2} declares {3}.".With(GetType().Name, RequiredOrdering, chromosome.GetType().Name, chromosome.GeneOrdering));
            }

            PerformMutate(chromosome, probability);
        }

        /// <summary>
        /// Mutate the specified chromosome.
        /// </summary>
        /// <param name="chromosome">The chromosome.</param>
        /// <param name="probability">The probability to mutate each chromosome.</param>
        protected abstract void PerformMutate(IChromosome chromosome, float probability);
        #endregion
    }
}
