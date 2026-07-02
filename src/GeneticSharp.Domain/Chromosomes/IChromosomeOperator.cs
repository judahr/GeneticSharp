namespace GeneticSharp
{
    /// <summary>
    /// Defines a basic interface for operators which works with chromosomes.
    /// </summary>
    public interface IChromosomeOperator
    {
        /// <summary>
        /// Gets the gene ordering a chromosome must declare (or exceed) for this operator to be valid on it.
        /// </summary>
        GeneOrdering RequiredOrdering { get; }
    }
}