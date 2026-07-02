namespace GeneticSharp
{
    /// <summary>
    /// The structural guarantee a chromosome makes about its genes, from loosest to strictest.
    /// </summary>
    /// <remarks>
    /// The levels are ordered: <see cref="Positional"/> &lt;= <see cref="Set"/> &lt;= <see cref="Permutation"/>.
    /// A chromosome satisfying a stricter level automatically satisfies any looser one, so an operator is
    /// compatible with a chromosome whenever <c>chromosome.GeneOrdering &gt;= operator.RequiredOrdering</c>.
    /// </remarks>
    public enum GeneOrdering
    {
        /// <summary>
        /// Each index is an independent named slot. Genes may repeat across slots and there is no
        /// requirement that all possible gene values appear. Any operator is valid.
        /// </summary>
        Positional = 0,

        /// <summary>
        /// Every gene must appear exactly once, but positions are interchangeable. Slot-swapping
        /// operators (e.g. Uniform, OnePoint, TwoPoint) break coverage and are invalid.
        /// </summary>
        Set = 1,

        /// <summary>
        /// Every gene must appear exactly once and the sequence itself encodes meaning. Only
        /// permutation-aware operators (OX1, PMX, CycleCrossover, etc.) are valid.
        /// </summary>
        Permutation = 2
    }
}
